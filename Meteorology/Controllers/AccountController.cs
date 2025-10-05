using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Controllers
{
    public class AccountController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public AccountController(
            GenericUoW genericUoW,
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IWebHostEnvironment environment)
        {
            _genericUoW = genericUoW;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.UserName.ToUpper(), model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                _genericUoW.Repository<UserLoginHistory>().Insert(new UserLoginHistory
                {
                    Type = EnuLoginHistory.Login,
                    UserId = user.Id
                });
                _genericUoW.Save(0, EnuAction.Create, "");

                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
                return RedirectToAction(nameof(Lockout));

            ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null) return RedirectToAction(nameof(Login));

            var model = new UserViewModel
            {
                User = _genericUoW.Repository<User>()
                    .GetAllByQuery(u => u.Id == user.Id, u => u.City).FirstOrDefault()
            };

            var userStations = _genericUoW.Repository<UserStation>()
                .GetAll(n => n.UserId == user.Id).ToList();

            model.ProjectList = _genericUoW.Repository<Project>()
                .GetAll(p => userStations.Any(us => us.ProjectId == p.Id))
                .OrderBy(p => p.Name)
                .Select(p => p.Name).ToList();

            model.StationList = _genericUoW.Repository<Station>()
                .GetAll(s => userStations.Any(us => us.StationId == s.Id))
                .OrderBy(s => s.Name)
                .Select(s => s.Name).ToList();

            model.UserActionList = _genericUoW.Repository<UserAction>()
                .GetAll(n => n.UserId == user.Id)
                .OrderByDescending(n => n.Id).ToList();

            ViewBag.StateList = new SelectList(
                _genericUoW.Repository<State>().GetAll().OrderBy(s => s.Name), "Id", "Name");

            var stateId = model.User.City != null ? model.User.City.StateId : 1;

            ViewBag.CityList = new SelectList(
                _genericUoW.Repository<City>()
                    .GetAll(c => c.StateId == stateId)
                    .OrderBy(c => c.Name), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(IFormCollection form, UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.User.Id.ToString());
            if (user == null) return RedirectToAction(nameof(Login));

            // Update basic info
            user.Name = model.User.Name;
            user.LastName = model.User.LastName;
            user.UserName = model.User.UserName;
            user.NormalizedUserName = model.User.UserName.ToUpper();
            if (!string.IsNullOrEmpty(model.User.PasswordHash))
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.User.PasswordHash);

            if (!string.IsNullOrEmpty(model.User.Email))
            {
                user.Email = model.User.Email;
                user.NormalizedEmail = model.User.Email.ToUpper();
            }

            user.PhoneNumber = model.User.PhoneNumber;
            user.CityId = Convert.ToInt64(form["CityId"]);

            // Update image if uploaded
            if (form.Files.Count > 0 && !string.IsNullOrEmpty(form.Files[0].FileName))
            {
                var uploadPath = Path.Combine(_environment.WebRootPath, "images", "users");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, form.Files[0].FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await form.Files[0].CopyToAsync(stream);

                user.Image = form.Files[0].FileName;
            }

            await _userManager.UpdateAsync(user);
            _genericUoW.Save(user.Id, EnuAction.Update, "اطلاعات پروفایل");

            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            await _signInManager.SignOutAsync();

            if (user != null)
            {
                _genericUoW.Repository<UserLoginHistory>().Insert(new UserLoginHistory
                {
                    Type = EnuLoginHistory.Logout,
                    UserId = user.Id
                });
                _genericUoW.Save(0, EnuAction.Create, "");
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout() => View();

        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetCities(int stateId)
        {
            var cities = _genericUoW.Repository<City>()
                .GetAll(c => c.StateId == stateId)
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name }).ToList();
            return Json(cities);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}
