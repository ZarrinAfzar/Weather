using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,SensorSetting")]
    public class SensorSettingController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;

        public SensorSettingController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
        }
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                return View();
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll( n => StationUserId.Any(x=>x.ProjectId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> SensorSetting_List(int ProjectId = 0 , int StationId = 0)
        {

            if(ProjectId == 0 && StationId == 0)
            {
                return PartialView(new List<SensorSetting>());
            }
            else
            {
                User user = await _userManager.GetUserAsync(HttpContext.User);
                Expression<Func<SensorSetting, object>>[] join = { n => n.Station, n => n.SensorTypes, n => n.Unit };
                Expression<Func<Station, bool>> whereStation = null;
                if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                {
                    if (ProjectId != 0)
                        whereStation = n => n.ProjectId == ProjectId;
                    if (StationId != 0)
                    {
                        var perfix = whereStation.Compile();
                        whereStation = n => perfix(n) && n.Id == StationId;
                    }
                }
                else
                {
                    List<UserStation> UserStations = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                    if (ProjectId != 0)  
                        whereStation = n => UserStations.Any(x=>x.ProjectId == ProjectId) && n.ProjectId == ProjectId;
                    if (StationId != 0)
                    {
                        var perfix = whereStation.Compile();
                        whereStation = n => perfix(n) && UserStations.Any(x => x.StationId == StationId) &&  n.Id == StationId;
                    }
                }
                List<long> lstStationId = _genericUoW.Repository<Station>().GetAllByQuery(whereStation.Compile()).Select(n => n.Id).ToList();
                List<SensorSetting> sensorSettings;
                if (await _userManager.IsInRoleAsync(user, "AdminiStratore")){
                     sensorSettings = _genericUoW.Repository<SensorSetting>().GetAllByQuery(n => lstStationId.Any(x => x == n.StationId) , join).OrderByDescending(n=>n.Id).ToList();
                }
                else
                {
                    sensorSettings = _genericUoW.Repository<SensorSetting>().GetAllByQuery(n => lstStationId.Any(x => x == n.StationId) && n.SensorEnable == true, join).ToList();

                }

                return PartialView(sensorSettings);
            }



        }

        [HttpGet]
        public IActionResult SensorSetting_Get(int SensorId)
        {
            SensorSetting row = _genericUoW.Repository<SensorSetting>().GetById(SensorId);
            SensorSettingViewModel model = new SensorSettingViewModel (){ Id = row.Id, SensorType = row.SensorTypeId.Value,Digit=row.SensorDigit??0, MinValue = row.SensorMin, MaxValue = row.SensorMax,UnitId=row.UnitId };


            ViewBag.SensorTypes =  new SelectList(_genericUoW.Repository<SensorType>().GetAll().Select(n => new { n.Id, n.ViewName }).OrderBy(n=>n.ViewName).ToList(), "Id", "ViewName");
            ViewBag.UnitList = new SelectList(_genericUoW.Repository<Unit>().GetAll().Select(u => new { u.Id, u.FaName }).OrderBy(m => m.FaName).ToList(), "Id", "FaName");
            return PartialView(model);
        }
        [HttpPost]
        public async Task<JsonResult> SensorSetting_Save(SensorSettingViewModel model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            SensorSetting row = _genericUoW.Repository<SensorSetting>().GetById(model.Id);
            row.SensorTypeId = model.SensorType;
            row.SensorDigit = model.Digit;
            row.SensorMax = model.MaxValue;
            row.SensorMin = model.MinValue;
            row.UnitId = model.UnitId;
            _genericUoW.Repository<SensorSetting>().Update(row);

            ViewBag.SensorTypes = new SelectList(_genericUoW.Repository<SensorType>().GetAll().Select(n => new { n.Id, n.ViewName }).OrderBy(m=>m.ViewName).ToList(), "Id", "ViewName");
            return Json(_genericUoW.Save(user.Id, EnuAction.Update,"تنظیمات سنسورها"));
        }


        [HttpPost]
        public async Task<JsonResult> GetStation(int Projectid)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m=>m.Name).ToList());
            else
            {
                List<long> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n=>n.StationId).ToList();
                return Json(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x=>x == n.Id) && n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(b => b.Name).ToList());
            }
        }
        public async Task<JsonResult> GetDefault(int SensorTypeId)
        {
            return Json(_genericUoW.Repository<SensorType>().GetAll(n => n.Id == SensorTypeId).First());
        }
    }
}