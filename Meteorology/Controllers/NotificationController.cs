using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,Notification")]
    public class NotificationController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;

        public NotificationController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _genericUoW = genericUoW;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
           
            
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                ViewBag.UserList = new SelectList(_genericUoW.Repository<User>().GetAll().Select(n => new { n.Id, n.UserName }).OrderBy(m => m.UserName).ToList(), "Id", "UserName");
                return View(_genericUoW.Repository<Notification>().GetAll(null, x => x.UserRecive, y => y.UserSend).ToList());
            }
            else
            {
                ViewBag.UserList = new SelectList(_genericUoW.Repository<User>().GetAll(n => n.UserInsertedId == user.Id).Select(n => new { n.Id, n.UserName }).OrderBy(m => m.UserName).ToList(), "Id", "UserName");
                var model = _genericUoW.Repository<Notification>().GetAll(n => n.UserGetId == user.Id, x => x.UserRecive, y => y.UserSend).OrderByDescending(u=>u.InsertDate).ToList();

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult NotificationPartial(int userId)
        {
            return PartialView(_genericUoW.Repository<Notification>().GetAll(n => (n.UserGetId == userId ) && !n.ViewState).OrderByDescending(u => u.InsertDate).ToList());
        }
        public int NotificationCount(int userId)
        {
            return (_genericUoW.Repository<Notification>().GetAll(n => (n.UserGetId == userId ) && !n.ViewState).Count());
        }
        [HttpPost]
        public  async Task<JsonResult> NotificationMessage(int NotificationId)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Notification notification = _genericUoW.Repository<Notification>().GetById(NotificationId);
            if (!(await _userManager.IsInRoleAsync(user, "AdminiStratore"))){
                notification.ViewState = true;
            }
            _genericUoW.Repository<Notification>().Update(notification);
            _genericUoW.Save(0, EnuAction.Update, "");
            return Json(notification.Message);
        }

        [HttpPost]
        public async Task<JsonResult> SendMessage(int[] UserGeter, string Message)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (UserGeter.Contains(0))
            {
                List<User> userids = _genericUoW.Repository<User>().GetAll(n => n.UserInsertedId == user.Id).ToList();
                foreach (var item in userids)
                {
                    Notification model = new Notification()
                    {
                        UserGetId = item.Id,
                        ViewState = false,
                        Message = Message,
                        UserInsertedId = user.Id
                    };
                    _genericUoW.Repository<Notification>().Insert(model);
                }
            }
            else if (UserGeter.Count() > 0)
            {
                foreach (var item in UserGeter)
                {
                    Notification model = new Notification()
                    {
                        UserGetId = item,
                        ViewState = false,
                        Message = Message,
                        UserInsertedId = user.Id
                    };
                    _genericUoW.Repository<Notification>().Insert(model);
                }
            }

            return Json(_genericUoW.Save(user.Id, EnuAction.Create, "پیام"));
        }

        [HttpGet]
        public async Task<IActionResult> Notification_List(string userNameSearch, string valueSearch, int sort = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new List<Notification>();
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                ViewBag.UserList = new SelectList(_genericUoW.Repository<User>().GetAll().Select(n => new { n.Id, n.UserName }).OrderBy(n=>n.UserName).ToList(), "Id", "UserName");
               model=_genericUoW.Repository<Notification>().GetAll(null, x => x.UserRecive, y => y.UserSend).OrderByDescending(m => m.InsertDate).ToList();
            }
            else
            {
                ViewBag.UserList = new SelectList(_genericUoW.Repository<User>().GetAll(n => n.UserInsertedId == user.Id).Select(n => new { n.Id, n.UserName }).OrderBy(n => n.UserName).ToList(), "Id", "UserName");
                 model = _genericUoW.Repository<Notification>().GetAll(n => n.UserGetId == user.Id, x => x.UserRecive, y => y.UserSend).OrderByDescending(m=>m.InsertDate).ToList();

               
            }
            if (!string.IsNullOrEmpty(userNameSearch))
            {

                model = model.FindAll(m => m.UserRecive.UserName.Contains(userNameSearch) || m.UserRecive.Name.Contains(userNameSearch) || m.UserRecive.LastName.Contains(userNameSearch)).ToList();

            }
            if (!string.IsNullOrEmpty(valueSearch))
            {

                model = model.FindAll(m => m.Message.Contains(valueSearch)).ToList();

            }

            switch (sort)
            {
                case 1:
                    model = model.OrderBy(n => n.Id).ToList();
                    break;
                case 2:
                    model = model.OrderByDescending(n => n.Id).ToList();
                    break;
                case 3:
                    model = model.OrderBy(n => n.InsertDate).ToList();
                    break;
                case 4:
                    model = model.OrderByDescending(n => n.InsertDate).ToList();
                    break;
                case 5:
                    model = model.OrderBy(n => n.UserRecive.UserName).ToList();
                    break;
                case 6:
                    model = model.OrderByDescending(n => n.UserRecive.UserName).ToList();
                    break;
                default:
                    break;
            }
            return PartialView(model);
        }
        [HttpGet]
        public async Task<JsonResult> NotificationDelete(int id)
        {
            if (id != 0)
            {
                User user = await _userManager.GetUserAsync(HttpContext.User);
                _genericUoW.Repository<Notification>().Delete(id);
                return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Delete, "پیام")));
            }
            return Json(false);
        }
    }
}