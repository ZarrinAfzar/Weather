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
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Weather.Controllers
{
    public class AccountController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly ILogger _logger;
        private readonly IHostingEnvironment _environment;

        public AccountController(
            GenericUoW genericUoW,
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            //ILogger<AccountController> logger,
            IHostingEnvironment environment)
        {
            _genericUoW = genericUoW;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            //_logger = logger;
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
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.UserName.ToUpper(), model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    User u = await _userManager.FindByNameAsync(model.UserName);
                    ///درج ورود کاربر در جدول رد پای کاربر
                    _genericUoW.Repository<UserLoginHistory>().Insert(new UserLoginHistory() { Type = EnuLoginHistory.Login, UserId = u.Id });
                    _genericUoW.Save(0, EnuAction.Create, "");
                    //_logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    //_logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور اشتباه است");
                    ViewBag.errorText = "نام کاربری یا رمز عبور اشتباه است";
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {

            ViewBag.StatectList = new SelectList(_genericUoW.Repository<State>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m =>  m.Name).ToList(), "Id", "Name");

            UserViewModel model = new UserViewModel();
            User user = await _userManager.GetUserAsync(HttpContext.User);
            model.User = _genericUoW.Repository<User>().GetAllByQuery(u => u.Id == user.Id, u => u.City).First();
            if (await _userManager.IsInRoleAsync(model.User, "AdminiStratore"))
            {
                model.UserActionList = _genericUoW.Repository<UserAction>().GetAll(n => n.UserId == model.User.Id).OrderByDescending(n => n.Id).ToList();
                ViewBag.CityList = model.User.City == null ? new SelectList(_genericUoW.Repository<City>().GetAll(c => c.StateId == 1).Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name") :
                    new SelectList(_genericUoW.Repository<City>().GetAll(c => c.StateId == model.User.City.StateId).Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
                return model != null ? View(model) : (IActionResult)RedirectToAction(nameof(Login));
            }
            ViewBag.CityList = model.User.City == null ? new SelectList(_genericUoW.Repository<City>().GetAll(c => c.StateId == 1).Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name") :
                new SelectList(_genericUoW.Repository<City>().GetAll(c => c.StateId == model.User.City.StateId).Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            List<UserStation> UserStations = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == model.User.Id).ToList();
            model.ProjectList = _genericUoW.Repository<Project>().GetAll(n => UserStations.Any(x => x.ProjectId == n.Id)).OrderBy(m => m.Name).Select(n => n.Name).ToList();
            model.StationList = _genericUoW.Repository<Station>().GetAll(n => UserStations.Any(x => x.StationId == n.Id)).OrderBy(m => m.Name).Select(n => n.Name).ToList();
            model.UserActionList = _genericUoW.Repository<UserAction>().GetAll(n => n.UserId == model.User.Id).OrderByDescending(n => n.Id).ToList();

            return model != null ? View(model) : (IActionResult)RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Profile(IFormCollection form, UserViewModel model)
        {


            User user = await _userManager.FindByIdAsync(model.User.Id.ToString());

            user.Id = model.User.Id;
            user.Name = model.User.Name;
            user.LastName = model.User.LastName;
            user.UserName = model.User.UserName;
            user.NormalizedUserName = model.User.UserName.ToUpper();
            user.PasswordHash = !string.IsNullOrEmpty(model.User.PasswordHash) ? _userManager.PasswordHasher.HashPassword(model.User, model.User.PasswordHash) : user.PasswordHash;
            if (model.User.Email != null)
            {
                user.Email = model.User.Email;
                user.NormalizedEmail = model.User.Email.ToUpper();
            }
            user.PhoneNumber = model.User.PhoneNumber;
            var CityId = form["CityId"];
            user.CityId = Convert.ToInt64(CityId);
            await _userManager.UpdateAsync(user);
            if (!string.IsNullOrEmpty(form.Files[0].FileName))
            {
                var webRoot = _environment.WebRootPath;
                if (!Directory.Exists($"{webRoot}/images/users/"))
                {
                    Directory.CreateDirectory($"{webRoot}/images/users/");
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"{webRoot}/images/users/", $"{form.Files[0].FileName}");
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await form.Files[0].CopyToAsync(stream);
                }
            }
            user.Image = !string.IsNullOrEmpty(form.Files[0].FileName) ? form.Files[0].FileName : user.Image;
            await _userManager.UpdateAsync(user);
            _genericUoW.Save(user.Id, EnuAction.Update, "اطلاعات پروفایل");


            List<UserStation> UserStations = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == model.User.Id).ToList();
            model.ProjectList = _genericUoW.Repository<Project>().GetAll(n => UserStations.Any(x => x.ProjectId == n.Id)).OrderBy(m => m.Name).Select(n => n.Name).ToList();
            model.StationList = _genericUoW.Repository<Station>().GetAll(n => UserStations.Any(x => x.StationId == n.Id)).OrderBy(m => m.Name).Select(n => n.Name).ToList();
            model.UserActionList = _genericUoW.Repository<UserAction>().GetAll(n => n.UserId == model.User.Id).OrderByDescending(n => n.Id).ToList();
            model.User = user;

            ViewBag.StatectList = new SelectList(_genericUoW.Repository<State>().GetAll().Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            ViewBag.CityList = new SelectList(_genericUoW.Repository<City>().GetAll().Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            return View(model);
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<JsonResult> SendRegisterCode(int userId, string RegisterCode)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user.RegisterCode == RegisterCode)
            {
                user.Registered = true;
                IdentityResult AddToRolesAsync_result = _userManager.UpdateAsync(user).Result;
                return AddToRolesAsync_result.Succeeded ? Json(true) : Json(false);
            }
            return Json(false);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserRegister(int userId)//async Task<>
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            Random random = new Random();
            user.RegisterCode = random.Next(10000, 99999).ToString();
            await _userManager.UpdateAsync(user);

            /////
            ///

            // _genericUoW.Repository<SmsToSend>().Insert(new SmsToSend() { InsertDate = DateTime.Now, PhoneNumber = user.PhoneNumber, Text = user.RegisterCode, State = false, UserId = user.Id });

            ViewBag.UserId = userId;
            ViewBag.RegisterCode = user.RegisterCode;
            ViewBag.PhoneNumber = user.PhoneNumber;
            return PartialView();
        }
       
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<UserLoginHistory>().Insert(new UserLoginHistory() { Type = EnuLoginHistory.Logout, UserId = user.Id });
            _genericUoW.Save(0, EnuAction.Create, "");
            return RedirectToAction(nameof(AccountController.Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region DRP
        [HttpGet]
        public JsonResult GetCities(int stateId)
        {
            //.Where(n => n.StateId == stateId)
            return Json(_genericUoW.Repository<City>().GetAll(n => n.StateId == stateId).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
        }


        #endregion
    }
}