using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,Project")]
    public class ProjectController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;

        public ProjectController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _genericUoW = genericUoW;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //int count = _genericUoW.Repository<Project>().GetAll().ToList().Count;
            //ViewBag.pagecount = (int)Math.Ceiling((double)count / 6);// Math.Ceiling((decimal)count);
            return View();
        }

          #region Project
        [HttpGet] 
        public async Task<IActionResult> Project_List(string valueSearch , int sort = 0 )
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var model = new List<Project>();
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                if (!string.IsNullOrEmpty(valueSearch))
                {
                    model = _genericUoW.Repository<Project>().GetAll(m => m.Name.Contains(valueSearch) || m.Detail.Contains(valueSearch), n => n.Stations).ToList();
                }
                else
                {
                    model = _genericUoW.Repository<Project>().GetAll(null, n => n.Stations).OrderBy(n=>n.Name).ToList();
                }
            }
            else
            {
                List<long> projectIds = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n=>n.ProjectId).ToList();
                if (!string.IsNullOrEmpty(valueSearch))
                {
                    model = _genericUoW.Repository<Project>().GetAll(n => projectIds.Any(x => x == n.Id) && n.Detail.Contains(valueSearch) || n.Name.Contains(valueSearch), n => n.Stations).OrderBy(n=>n.Name).ToList();
                }
                else
                {
                    model = _genericUoW.Repository<Project>().GetAll(n => projectIds.Any(x => x == n.Id), n => n.Stations).OrderBy(n=>n.Name).ToList();
                }

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
                    model = model.OrderBy(n => n.Name).ToList();
                    break;
                case 6:
                    model = model.OrderByDescending(n => n.Name).ToList();
                    break;
                default:
                    break;
            }
            return PartialView("Project_List", model);

        }
        [HttpGet]
        public IActionResult Get_Project(int? id)
        {
            return id != null ? PartialView("Project_Get",_genericUoW.Repository<Project>().GetById(id.Value)) : PartialView("Project_Get",new Project());
        }
        [HttpGet]
        public IActionResult Project_View(int id)
        {
            return PartialView("Project_View", _genericUoW.Repository<Project>().GetById(id));
        }
        [HttpGet]
        public async Task<IActionResult> Project_Delete(int id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (id != 0)
            {
                _genericUoW.Repository<Project>().Delete(id);
                return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Delete, "پروژه")));
            }
            return Json(false);
        }
        [HttpPost]
        public async Task<IActionResult> Project_Save([FromBody] Project model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (model.Id == 0)
            {
                _genericUoW.Repository<Project>().Insert(model);
                return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Create, "پروژه")));
            }
            else
            {
                Project oldModel = _genericUoW.Repository<Project>().GetById(model.Id);
                _genericUoW.Repository<Project>().Update(model, oldModel);
                return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Update, "پروژه")));
            }
        }
        #endregion

        #region SmsCharge
        [HttpGet]
        public IActionResult Project_SmsRequest(int id)
        {
            int SMSCount= 0;
            List<Station> stations = _genericUoW.Repository<Station>().GetAllByQuery(n => n.ProjectId == id).ToList();
            foreach (var item in stations)
            {
                SMSCount += item.SmsCount??0;
            }
            ViewBag.AllSms = SMSCount;
            return PartialView("Project_SmsRequest", _genericUoW.Repository<Project>().GetById(id));
        }
        [HttpPost]
        public async Task<JsonResult> SmsRequest_Save(int ProjectId,int oldCount, int smsCount)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var oldmodel = _genericUoW.Repository<Project>().GetById(ProjectId);
            oldmodel.SmsCount = oldCount+smsCount;
            _genericUoW.Repository<Project>().Update(oldmodel);
            _genericUoW.Repository<SmsCharge>().Insert(new SmsCharge() { ProjectId = ProjectId, Count = smsCount });
            return Json(_genericUoW.Save(user.Id, EnuAction.Update, "پیامک های پروژه"));
        }

        [HttpGet]
        public IActionResult SmsCharge()
        {
            int SMSCount = 0;
            List<Station> stations = _genericUoW.Repository<Station>().GetAll().ToList();
            foreach (var item in stations)
            {
                SMSCount += item.SmsCount ?? 0;
            }
            List<Project> prj = _genericUoW.Repository<Project>().GetAll();
            foreach (var item in prj)
            {
                SMSCount += item.SmsCount;
            }
            ViewBag.SmsPsnel = SMSCount;
            return View(_genericUoW.Repository<SmsCharge>().GetAll(null, n => n.Project));
        }
        #endregion

        #region SmsSetToStation
        [HttpGet]
        public IActionResult Project_SmsSetToStation(int id)
        {
            return PartialView("Project_SmsSetToStation", new Project());
        }
        public async Task<JsonResult> Project_SmsSetToStation([FromBody] SmsSetToStationVm model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var newStation = _genericUoW.Repository<Station>().GetById(model.StationId);
            newStation.SmsCount = model.SmsCount;
            newStation.ChargeSms = DateTime.Now;
            _genericUoW.Repository<Station>().Update(newStation);


            var project = _genericUoW.Repository<Project>().GetById(model.ProjectId);
            project.SmsCount -= model.SmsCount;
            _genericUoW.Repository<Project>().Update(project);

            return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Update, "پیامک برای ایستگاه")));
        }
        #endregion
    }
}