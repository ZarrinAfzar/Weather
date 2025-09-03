using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,Station")]
    public class StationController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _environment;

        public StationController(GenericUoW genericUoW, UserManager<User> userManager, IHostingEnvironment environment)
        {
            _genericUoW = genericUoW;
            _userManager = userManager;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            //ViewBag.DataLoggerList = new SelectList(_genericUoW.Repository<DataLogger>().GetAll().Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            else
            {
                List<long> ProjectId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.ProjectId).ToList();
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => ProjectId.Any(x => x == n.Id)).OrderBy(m => m.Name).Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            }
            //ViewBag.StationTypeList = new SelectList(_genericUoW.Repository<StationType>().GetAll().Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            //ViewBag.ModemTypeList = new SelectList(_genericUoW.Repository<ModemType>().GetAll().Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            return View();
        }



        #region ProjectInfo
        [HttpGet]
        public IActionResult StationGetProjectInfo(int id)
        {
            return PartialView("StationProject_View", _genericUoW.Repository<Project>().GetById(id));
        }
        #endregion

        #region Station 
        [HttpPost]
        public async Task<IActionResult> Station_List(int?[] projectIds)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            List<Station> model = new List<Station>();
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                if (projectIds.Count() > 0 && projectIds[0] != null)
                {
                    model = _genericUoW.Repository<Station>().GetAll(n => projectIds.Any(x => x == n.ProjectId), n => n.Project).OrderBy(m => m.Name).ToList();
                }
                else
                {
                    model = _genericUoW.Repository<Station>().GetAll(null, n => n.Project).OrderBy(m => m.Name).ToList();
                }
            }
            else
            {
                List<long> StationId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
                if (projectIds.Count() > 0 && projectIds[0] != null)
                {
                    model = _genericUoW.Repository<Station>().GetAll(n => StationId.Any(x => x == n.Id) && projectIds.Any(y => y == n.ProjectId), n => n.Project).OrderBy(m => m.Name).ToList();
                }
                else
                {
                    model = _genericUoW.Repository<Station>().GetAll(n => StationId.Any(x => x == n.Id), n => n.Project).OrderBy(m => m.Name).ToList();
                }
            }
            model = model.OrderBy(n => n.Project.Name).ThenBy(m => m.Name).ToList();
            return PartialView("Station_List", model);
        }
        [HttpGet]
        public IActionResult Station_Detail(int? id)
        {

            return id != null
                ? PartialView("Station_Detail", _genericUoW.Repository<Station>().GetById(id.Value))
                : PartialView("Station_Detail", _genericUoW.Repository<Station>().GetAll().FirstOrDefault());
        }
        [HttpGet]
        public IActionResult Station_Get(int? id)
        {
            var model = new Station();
            if (id != null)
            {
                model = _genericUoW.Repository<Station>().GetById(id.Value);
            }
            ViewBag.DataLoggerList = new SelectList(_genericUoW.Repository<DataLogger>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            ViewBag.StationTypeList = new SelectList(_genericUoW.Repository<StationType>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            ViewBag.ModemTypeList = new SelectList(_genericUoW.Repository<ModemType>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            return PartialView("Station_Get", model);
        }
        [HttpGet]
        public async Task<JsonResult> Station_Delete(int id)
        {
            if (id != 0)
            {
                User user = await _userManager.GetUserAsync(HttpContext.User);
                _genericUoW.Repository<Station>().Delete(id);
                return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Delete, "ایستگاه")));
            }
            return Json(false);
        }
        [HttpPost]
        public JsonResult Station_Serial(string serial)
        {

            if (_genericUoW.Repository<Station>().GetAll().Any(s => s.SerialNumber == serial))
            {

                return Json(Convert.ToBoolean(true));
            }
            else
            {
                return Json(Convert.ToBoolean(false));

            }
        }

        [HttpPost]
        public async Task<IActionResult> Station_Save([FromBody] Station model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (model.Id == 0)
            {
                _genericUoW.Repository<Station>().Insert(model);
                return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Create, "ایستگاه")));
            }
            else
            {
                Station oldModel = _genericUoW.Repository<Station>().GetById(model.Id);
                _genericUoW.Repository<Station>().Update(model, oldModel);
                return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Update, "ایستگاه")));
            }
        }
        #endregion

        #region StationTel
        [HttpGet]
        public IActionResult StationTel_List(int StationId)
        {
            return PartialView("Station_Tel", _genericUoW.Repository<StationTel>().GetAll(n => n.StationId == StationId).ToList());
        }
        [HttpGet]
        public JsonResult StationTel_Get(int id)
        {
            return Json(_genericUoW.Repository<StationTel>().GetById(id));
        }
        [HttpPost]
        public async Task<JsonResult> StationTel_Save(long id, int Stationid, string Name, string LastName, string Post, string Tel)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var model = new StationTel()
            {
                Id = id,
                Name = Name,
                LastName = LastName,
                StationId = Stationid,
                Post = Post,
                Tel = Tel
            };
            if (model.Id == 0)
            {
                _genericUoW.Repository<StationTel>().Insert(model);
                return Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Create, "تلفن های ایستگاه")) ? Json(model) : Json(false);
            }
            else
            {
                _genericUoW.Repository<StationTel>().Update(model);
                return Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Update, "تلفن های ایستگاه")) ? Json(model) : Json(false);
            }
        }
        public async Task<JsonResult> StationTel_Delete(int Id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<StationTel>().Delete(Id);
            return Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Delete, "تلفن های ایستگاه")) ? Json(Id) : Json(false);
        }

        #endregion

        
        #region StationFile
        [HttpGet]
        public IActionResult StationFile_List(int StationId)
        {
            return PartialView("Station_File", _genericUoW.Repository<StationFile>().GetAll(n => n.StationId == StationId).ToList());
        }
        [HttpGet]
        public IActionResult StationFile_Get(int Id)
        {
            return Json(_genericUoW.Repository<StationFile>().GetById(Id));
        }
        [HttpPost]
        public async Task<JsonResult> StationFile_Save(IFormFile file, StationFile model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (file != null && file.Length > 0)
            {
                var webRoot = _environment.WebRootPath;
                if (!Directory.Exists(webRoot + "/StationFiles/"))
                {
                    Directory.CreateDirectory(webRoot + "/StationFiles/");
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/StationFiles/", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }


            if (model.Id == 0)
            {
                model.FileAddress = file.FileName;
                _genericUoW.Repository<StationFile>().Insert(model);
                Notification notification = new Notification();
                notification.InsertDate = DateTime.Now;
                notification.Message = "کاربر: " + user.UserName + " یک فایل برای ایستگاه اضافه کرد";
                notification.UserGetId = 1;
                notification.ViewState = false;
                _genericUoW.Repository<Notification>().Insert(notification);
                return Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Create, "فایل های ایستگاه")) ? Json(model) : Json(false);
            }
            else
            {
                var old = _genericUoW.Repository<StationFile>().GetById(model.Id);
                model.FileAddress = file == null ? old.FileAddress : file.FileName;
                _genericUoW.Repository<StationFile>().Update(model, old);
                return Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Update, "فایل های ایستگاه")) ? Json(model) : Json(false);
            }
        }
        [HttpGet]
        public async Task<JsonResult> StationFile_Delete(int Id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<StationFile>().Delete(Id);
            return Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Delete, "فایل های ایستگاه")) ? Json(Id) : Json(false);
        }
        #endregion

        #region StationSms
        [HttpGet]
        public IActionResult StationSms(int StationId)
        {
            Station station = _genericUoW.Repository<Station>().GetAll(n => n.Id == StationId, n => n.Project).FirstOrDefault();
            StationSmsViewModel model = new StationSmsViewModel()
            {
                StationId = StationId,
                StationName = station.Name,
                ProjectName = station.Project.Name,
                ProjectSmsCount = station.Project.SmsCount,
                StationSmsCount = station.SmsCount == null ? 0 : station.SmsCount.Value,
            };
            return PartialView(model);
        }
        [HttpPost]
        public async Task<JsonResult> StationSmsUpdate(int StationId, int SmsCount)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var station = _genericUoW.Repository<Station>().GetById(StationId);
            var project = _genericUoW.Repository<Project>().GetById(station.ProjectId);
            if (project.SmsCount > SmsCount)
            {
                station.SmsCount += SmsCount;
                project.SmsCount -= SmsCount;

                return Json(_genericUoW.Save(user.Id, EnuAction.Update, "تعداد پیامک ایستگاه"));

            }
            else
                return Json(false);
        }

        #endregion

    }
}