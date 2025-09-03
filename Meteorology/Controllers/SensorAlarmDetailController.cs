using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,SensorAlarmDetail")]
    public class SensorAlarmDetailController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
       
        public SensorAlarmDetailController(GenericUoW genericUoW, UserManager<User> userManager)
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

                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> SensorAlarmDetail_List(int ProjectId = 0, int StationId = 0, int AlarmId = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Expression<Func<SensorAlarmDetail, object>>[] join = { n => n.SensorType, n => n.Alarm, n => n.Alarm.Station, n => n.Alarm.Station.Project };
            IEnumerable<SensorAlarmDetail> model;
            List<long> StationUserId;
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                StationUserId = _genericUoW.Repository<Station>().GetAll().Select(n => n.Id).ToList();
            else
            {
                StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
            }

            Expression<Func<SensorAlarmDetail, bool>> SensorAlarmDetail_where = c => c.Alarm.AlarmType == EnuAlarm.Sensors && StationUserId.Any(x => x == c.Alarm.StationId);


            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                if (ProjectId != 0)
                {
                    SensorAlarmDetail_where = c => c.Alarm.AlarmType == EnuAlarm.Sensors;
                    List<long> StationWithProjectId = _genericUoW.Repository<Station>().GetAll(n => n.ProjectId == ProjectId).Select(n => n.Id).ToList();
                    List<long> AlarmIdList_Project = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationWithProjectId.Any(x => x == n.StationId)).Select(n => n.Id).ToList();
                    var prefix = SensorAlarmDetail_where.Compile();
                    SensorAlarmDetail_where = c => prefix(c) && AlarmIdList_Project.Any(n => n == c.AlarmId);
                }
                if (StationId != 0)
                {
                    List<long> AlarmIdList_Station = _genericUoW.Repository<Alarm>().GetAllByQuery(n => n.StationId == StationId).Select(n => n.Id).ToList();
                    var prefix = SensorAlarmDetail_where.Compile();
                    SensorAlarmDetail_where = c => prefix(c) && AlarmIdList_Station.Any(n => n == c.AlarmId);
                }
                if (AlarmId != 0)
                {
                    var prefix = SensorAlarmDetail_where.Compile();
                    SensorAlarmDetail_where = c => prefix(c) && c.AlarmId == AlarmId;
                }

            }
            else
            {
                List<UserStation> StationUserIdList = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                if (ProjectId != 0)
                {
                    SensorAlarmDetail_where = c => c.Alarm.AlarmType == EnuAlarm.Sensors;
                    List<long> StationWithProjectId = _genericUoW.Repository<Station>().GetAll(n => StationUserIdList.Any(x => x.ProjectId == n.ProjectId) && n.ProjectId == ProjectId).Select(n => n.Id).ToList();
                    List<long> AlarmIdList_Project = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationWithProjectId.Any(x => x == n.StationId)).OrderByDescending(n=>n.Id).Select(n => n.Id).ToList();
                    var prefix = SensorAlarmDetail_where.Compile();
                    SensorAlarmDetail_where = c => prefix(c) && AlarmIdList_Project.Any(n => n == c.AlarmId);
                }
                if (StationId != 0)
                {
                    List<long> AlarmIdList_Station = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationUserIdList.Any(x => x.StationId == n.StationId) && n.StationId == StationId).Select(n => n.Id).ToList();
                    var prefix = SensorAlarmDetail_where.Compile();
                    SensorAlarmDetail_where = c => prefix(c) && AlarmIdList_Station.Any(n => n == c.AlarmId);
                }
                if (AlarmId != 0)
                {
                    var prefix = SensorAlarmDetail_where.Compile();
                    SensorAlarmDetail_where = c => prefix(c) && c.AlarmId == AlarmId;
                }

            }
            try
            {
                model = _genericUoW.Repository<SensorAlarmDetail>().GetAllByQuery(SensorAlarmDetail_where.Compile(), join);

            }
            catch (Exception ex)
            {
                throw;
            }
            return PartialView(model.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> SensorAlarmDetail_Get(int id = 0, int SensorTypeId = 0, int StationId = 0, int AlarmId = 0)
        {
            List<SensorAlarmDetail> DetailsList = _genericUoW.Repository<SensorAlarmDetail>().GetAll(a => a.AlarmId == AlarmId);
            SensorAlarmDetail model = new SensorAlarmDetail();

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll().ToList();
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
            }
            List<long?> sensorIds = new List<long?>();
            if (SensorTypeId != 0)
            {
                sensorIds = _genericUoW.Repository<SensorSetting>().GetAll(n => n.StationId == StationId && n.SensorTypeId == SensorTypeId).Select(n => n.SensorTypeId).ToList();
            }
            else
            {
                sensorIds = _genericUoW.Repository<SensorSetting>().GetAll(n => n.StationId == StationId && n.SensorEnable).Select(n => n.SensorTypeId).ToList();
            }
            ViewBag.SensorList = new SelectList(_genericUoW.Repository<SensorType>().GetAll(n => sensorIds.Any(x => x == n.Id)).Select(n => new { n.Id, n.FaName }).OrderBy(n=>n.FaName).ToList(), "Id", "FaName");
            ViewBag.AlarmId = AlarmId;
            if (DetailsList.Count > 0)
            {
                model.OnlineOrTimeDuration = DetailsList[0].OnlineOrTimeDuration;
                if (!DetailsList[0].OnlineOrTimeDuration)
                    model.TimeDuration = DetailsList[0].TimeDuration;
                else
                    model.Threshold = null;
            }
           
            return PartialView(id == 0 ? model : _genericUoW.Repository<SensorAlarmDetail>().GetById(id));
        }
        [HttpPost]
        public async Task<JsonResult> SensorAlarmDetail_Save(SensorAlarmDetail model)
        {

            User user = await _userManager.GetUserAsync(HttpContext.User);


            if (model.Id == 0)
            {
                _genericUoW.Repository<SensorAlarmDetail>().Insert(model);
                return Json(_genericUoW.Save(user.Id, EnuAction.Create, "جزئیات آلارم سنسورها"));
            }
            else
            {
                _genericUoW.Repository<SensorAlarmDetail>().Update(model);
                return Json(_genericUoW.Save(user.Id, EnuAction.Update, "جزئیات آلارم سنسورها"));
            }
        }
        [HttpGet]
        public async Task<JsonResult> SensorAlarmDetail_Delete(int id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<SensorAlarmDetail>().Delete(id);
            return Json(_genericUoW.Save(user.Id, EnuAction.Delete, "جزئیات آلارم سنسورها"));
        }


        [HttpPost]
        public async Task<JsonResult> GetStation(int Projectid)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(n => n.Name).ToList());
            else
            {
                List<long> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
                return Json(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x == n.Id) && n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(n => n.Name).ToList());
            }
        }
        [HttpPost]
        public JsonResult GetAlarm(int Stationid)
        {
            var alramList = _genericUoW.Repository<Alarm>().GetAll(n => n.StationId == Stationid && n.AlarmType == EnuAlarm.Sensors).Select(n => new { n.Id, n.AlarmName }).ToList();
            List<long?> sensorIds = _genericUoW.Repository<SensorSetting>().GetAll(n => n.StationId == Stationid).OrderBy(n => n.SensorName).Select(n => n.SensorTypeId).ToList();
            var sensortypeList = _genericUoW.Repository<SensorType>().GetAll(n => sensorIds.Any(x => x == n.Id)).OrderBy(n => n.FaName).Select(n => new { n.Id, n.FaName }).ToList();
            return Json(new { alramList, sensortypeList });
        }

    }
}