using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Weather.Data.Base;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,User")]
    public class UserController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;

        public UserController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _genericUoW = genericUoW;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            int count = 0;
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                count = _genericUoW.Repository<User>().GetAll().ToList().Count;
            }
            else
            {
                count = _genericUoW.Repository<User>().GetAll(n => n.UserInsertedId != null && n.UserInsertedId == user.Id && n.Id != user.Id).ToList().Count;
            }
            ViewBag.pagecount = (int)Math.Ceiling((double)count / 6);
            return View();
        }


        //--------------Sub Forms-----------------------------

        #region User
        [HttpGet]
        public async Task<IActionResult> User_List(string valueSearch, int sort = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            List<User> model = new List<User>();
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                model = _genericUoW.Repository<User>().GetAll(n => n.Id != user.Id, n => n.City).ToList();
            }
            else
            {
                model = AllChild(user);

            }

            try
            {
                if (!string.IsNullOrEmpty(valueSearch))
                {

                    model = model.FindAll(m => m.Name.Contains(valueSearch) || m.LastName.Contains(valueSearch) || m.UserName.Contains(valueSearch)).ToList();

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
                        model = model.OrderBy(n => n.UserName).ToList();
                        break;
                    case 6:
                        model = model.OrderByDescending(n => n.UserName).ToList();
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("User_List", model);
        }
        public List<User> AllChild(User user)
        {
            var model = _genericUoW.Repository<User>().GetAll(n => n.UserInsertedId != null && n.UserInsertedId == user.Id, n => n.City).ToList();
            var ListUser = new List<User>();
            if (model != null)
                ListUser.AddRange(model);
            foreach (var item in model)
            {
                var childmodel = _genericUoW.Repository<User>().GetAll(n => n.UserInsertedId != null && n.UserInsertedId == item.Id, n => n.City).ToList();
                if (childmodel != null)
                    ListUser.AddRange(childmodel);
            }

            return ListUser;
        }
        [HttpGet]
        public IActionResult User_Get(int? id)
        {
            User model = id != null ? _genericUoW.Repository<User>().GetById(id.Value) : new User();
            ViewBag.StatectList = new SelectList(_genericUoW.Repository<State>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(s => s.Name).ToList(), "Id", "Name");
            ViewBag.CityList = new SelectList(_genericUoW.Repository<City>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(c => c.Name).ToList(), "Id", "Name");
            return PartialView("User_Get", model);
        }
        [HttpGet]
        public async Task<JsonResult> User_Delete(int id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (id != 0)
            {
                _genericUoW.Repository<User>().Delete(id);
                return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Delete, "کاربر")));
            }
            return Json(false);
        }
        [HttpGet]
        public JsonResult City(int stateId)
        {
            //.Where(n => n.StateId == stateId)
            return Json(_genericUoW.Repository<City>().GetAll(n => n.StateId == stateId).OrderBy(c => c.Name).ToList());
        }
        [HttpPost]
        public IActionResult User_Save([FromBody] User model)
        {
            if (model.Id == 0)
            {
                model.UserName = string.IsNullOrEmpty(model.UserName) ? model.PhoneNumber : model.UserName;
                string Password = string.IsNullOrEmpty(model.PasswordHash) ? model.PhoneNumber : model.PasswordHash;
                model.PasswordHash = _userManager.PasswordHasher.HashPassword(model, Password);
                Notification Msg = new Notification();
                Msg.InsertDate = DateTime.Now;
                Msg.Message = ".کاربر گرامی خوش آمدید. لطفا جهت استفاده از سایت ابتدا از قسمت ارتباطات/دانلود ها، راهنمای استفاده سایت را دانلود نموده و مطالعه بفرمایید.   ";
                Msg.UserGetId = model.Id;
                Msg.UserInsertedId = 1;
                Msg.ViewState = false;

                IdentityResult CreateAsync_result = _userManager.CreateAsync(model).Result;
                if (CreateAsync_result.Succeeded)
                {
                    IEnumerable<string> RoleBase = Enum.GetNames(typeof(EnuRoleBase));
                    IdentityResult AddToRolesAsync_result = _userManager.AddToRolesAsync(model, RoleBase).Result;
                    if (AddToRolesAsync_result.Succeeded)
                    {
                        _genericUoW.Repository<Notification>().Insert(Msg);
                        return Json(true);
                    }
                    else return Json(false);
                }
                if (CreateAsync_result.Succeeded) _genericUoW.Save(model.Id, EnuAction.Create, "کاربر");
                return Json(false);
            }
            else
            {
                var _user = _genericUoW.Repository<User>().GetById(model.Id);
                _user.PasswordHash = !string.IsNullOrEmpty(model.PasswordHash) ? _userManager.PasswordHasher.HashPassword(model, model.PasswordHash) : _user.PasswordHash;
                _user.Id = model.Id;
                _user.Name = model.Name;
                _user.LastName = model.LastName;
                _user.UserName = model.UserName;
                _user.PhoneNumber = model.PhoneNumber;
                _user.Email = model.Email;
                _user.CityId = model.CityId;
                _user.UserInsertedId = _user.UserInsertedId;
                _user.Registered = _user.Registered;
                _user.RegisterCode = _user.RegisterCode;

                IdentityResult UpdateAsync_result = _userManager.UpdateAsync(_user).Result;
                if (UpdateAsync_result.Succeeded) _genericUoW.Save(model.Id, EnuAction.Update, "کاربر");
                return UpdateAsync_result.Succeeded ? Json(true) : Json(false);
            }
        }
        #endregion

        #region Correspondence
        [HttpGet]
        public IActionResult User_CorrespondenceAnswer(int id)
        {
            var model = _genericUoW.Repository<Correspondence>().GetAll(n => n.UserSenderId == id).ToList();
            foreach (var item in model)
            {
                if (!item.ViewState)
                {
                    item.ViewState = true;
                    _genericUoW.Repository<Correspondence>().Update(item);
                    _genericUoW.Save(0, EnuAction.Create, "");
                }
            }
            return PartialView("User_CorrespondenceAnswer", model);
        }
        [HttpGet]
        public IActionResult GetMessageForAnswer(int id)
        {
            string txt = _genericUoW.Repository<Correspondence>().GetById(id).MessageText;
            var model = new Correspondence()
            {
                MessageAnswerId = id,
                OldTxt = txt
            };
            return PartialView("User_CorrespondenceAnswerForm", model);
        }
        public async Task<JsonResult> DeleteMessageForAnswer(int id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<Correspondence>().Delete(id);
            return Json(_genericUoW.Save(user.Id, EnuAction.Delete, "پاسخ به پیشنهادات و انتقادات"));
        }
        [HttpPost]
        public async Task<JsonResult> Save_MessageForAnswer(int MessageAnswerId, string MessageText)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Correspondence model = new Correspondence()
            {
                ViewState = true,
                UserSenderId = user.Id,
                MessageAnswerId = MessageAnswerId,
                MessageText = MessageText,
            };

            _genericUoW.Repository<Correspondence>().Insert(model);
            return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Create, "پاسخ به پیشنهادات و انتقادات")));
        }
        #endregion

        #region Station
        [HttpGet]
        public async Task<IActionResult> User_Stations(int id)
        {
            User User = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(User, "AdminiStratore"))
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            else
            {
                List<long> userproject = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == User.Id).Select(n => n.ProjectId).ToList();
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => userproject.Any(x => x == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            }
            var ids = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == id).Select(n => n.StationId).ToList();
            var model = _genericUoW.Repository<Station>().GetAll(n => ids.Any(x => x == n.Id)).ToList();
            return PartialView("User_Stations", model);
        }
        [HttpGet]
        public async Task<IActionResult> GetStation(int id, int userId)
        {
            User User = await _userManager.GetUserAsync(HttpContext.User);



            List<object> model = new List<object>();
            List<Station> allStation = new List<Station>();
            List<long> userStation_StationId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == userId).Select(n => n.StationId).ToList();
            if (await _userManager.IsInRoleAsync(User, "AdminiStratore"))
                allStation = _genericUoW.Repository<Station>().GetAll(n => n.ProjectId == id).ToList();
            else
            {
                List<long> allowid = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == User.Id).Select(n => n.StationId).ToList();
                allStation = _genericUoW.Repository<Station>().GetAll(n => allowid.Any(x => x == n.Id) && n.ProjectId == id).OrderBy(n => n.Name).ToList();
            }
            foreach (var item in allStation)
            {
                var i = new { Id = item.Id, Name = item.Name, state = userStation_StationId.Any(n => n == item.Id) ? true : false };
                model.Add(i);
            }
            return Json(model);
        }
        [HttpPost]
        public async Task<JsonResult> SetStationToUser(int userId, int stationId, int projectId, bool state)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (state)
            {
                UserStation model = new UserStation()
                {
                    UserId = userId,
                    StationId = stationId,
                    ProjectId = projectId,
                };
                _genericUoW.Repository<UserStation>().Insert(model);
                return Json(_genericUoW.Save(user.Id, EnuAction.Create, "ایستگاه های کاربر"));
            }
            else
            {
                UserStation model = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == userId && n.StationId == stationId && n.ProjectId == projectId).FirstOrDefault();
                _genericUoW.Repository<UserStation>().Delete(model);
                return Json(_genericUoW.Save(user.Id, EnuAction.Delete, "ایستگاه های کاربر"));
            }
        }
        #endregion

        #region Access
        [HttpGet]
        public async Task<IActionResult> User_AccessAsync(int id)
        {
            List<UserAccessViewModel> model = new List<UserAccessViewModel>();
            Array AllRoles;
            User ThisUser = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(ThisUser, "AdminiStratore"))
            {
                AllRoles = Enum.GetValues(typeof(EnuRole));

            }
            else
            {
                var ThisUserRoles = await _userManager.GetRolesAsync(ThisUser);
                AllRoles = Enum.GetValues(typeof(EnuRole)).Cast<EnuRole>().Where(n => ThisUserRoles.Any(x => x == Enum.GetName(typeof(EnuRole), n).ToString())).ToArray();
            }
            User user = await _userManager.FindByIdAsync(id.ToString());
            var userRole = await _userManager.GetRolesAsync(user);
            foreach (var item in AllRoles)
            {
                UserAccessViewModel m = new UserAccessViewModel()
                {
                    UserId = user.Id,
                    Value = Enum.GetName(typeof(EnuRole), item),
                    Description = ((Enum)item).GetDisplayName(),
                    State = userRole.Any(n => n == Enum.GetName(typeof(EnuRole), item).ToString()) ? true : false,
                };
                model.Add(m);
            }
            return PartialView("User_Access", model);
        }
        [HttpPost]
        public async Task<JsonResult> Change_UserRole(string Userid, string Rolename, bool State)
        {
            User user = await _userManager.FindByIdAsync(Userid);
            if (State)
            {
                IdentityResult CreateAsync_result = _userManager.AddToRoleAsync(user, Rolename).Result;
                return CreateAsync_result.Succeeded ? Json(true) : Json(false);
            }
            else
            {
                IdentityResult CreateAsync_result = _userManager.RemoveFromRoleAsync(user, Rolename).Result;
                return CreateAsync_result.Succeeded ? Json(true) : Json(false);
            }
        }
        #endregion

        #region State
        [HttpPost]
        public async Task<JsonResult> UserState(int userId, bool state)
        {
            User inuser = await _userManager.GetUserAsync(HttpContext.User);
            var user = _genericUoW.Repository<User>().GetById(userId);
            user.Registered = state;
            _genericUoW.Repository<User>().Update(user);
            return Json(_genericUoW.Save(inuser.Id, EnuAction.Update, "وضعیت کاربر"));
        }
        #endregion

        #region Notification
        [HttpGet]
        public IActionResult User_Notification(int UserId)
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<JsonResult> User_Notification_Save(int UserId, string Message)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Notification model = new Notification()
            {
                Message = Message,
                UserGetId = UserId,
                UserInsertedId = user.Id,
                ViewState = false
            };
            _genericUoW.Repository<Notification>().Insert(model);
            return Json(_genericUoW.Save(user.Id, EnuAction.Create, "پیام ها"));
        }
        #endregion
    }
}