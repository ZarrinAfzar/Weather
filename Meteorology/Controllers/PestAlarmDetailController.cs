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
    [Authorize(Roles = "AdminiStratore,PestAlarmDetail")]
    public class PestAlarmDetailController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public PestAlarmDetailController(GenericUoW genericUoW, UserManager<User> userManager)
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
        public async Task<IActionResult> PestAlarmDetail_List(int ProjectId = 0, int StationId = 0, int AlarmId = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Expression<Func<PestAlarmDetail, object>>[] join = { n => n.Alarm, n => n.Alarm.Station, n => n.Alarm.Station.Project };
            Expression<Func<PestAlarmDetail, bool>> where = null;
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                if (ProjectId != 0 && StationId != 0 && AlarmId != 0)
                {

                    where = n => n.Alarm.Station.ProjectId == ProjectId && n.AlarmId == AlarmId && n.Alarm.Station.Id == StationId;
                }
                else if (ProjectId != 0 && StationId != 0)
                {
                    where = n => n.Alarm.Station.Id == StationId;
                }
                else if (ProjectId != 0)
                {
                    where = n => n.Alarm.Station.ProjectId == ProjectId;
                }
              
                else if (AlarmId != 0)
                {
                    where = n => n.AlarmId == AlarmId;
                }
                else
                {
                    
                      return PartialView(_genericUoW.Repository<PestAlarmDetail>().GetAllByQuery(null, join).ToList());

                }
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                if (ProjectId != 0 && StationId != 0 && AlarmId != 0)
                {
                    where = n => StationUserId.Any(x => x.ProjectId == ProjectId && x.StationId == StationId) && n.Alarm.Station.ProjectId == ProjectId && n.AlarmId == AlarmId && n.Alarm.Station.Id == StationId;
                }

                else if (ProjectId != 0 && StationId != 0)
                {
                    where = n => StationUserId.Any(x => x.StationId == StationId) && n.Alarm.Station.Id == StationId;
                }
                else if (ProjectId != 0)
                {
                    where = n => StationUserId.Any(x => x.ProjectId == ProjectId) && n.Alarm.Station.ProjectId == ProjectId;
                }
                              
                else
                {
                    where = n => StationUserId.Any(x => x.StationId == n.Alarm.StationId);
                    return PartialView(_genericUoW.Repository<PestAlarmDetail>().GetAllByQuery(where.Compile(), join).ToList());
                }

            }
            return PartialView(_genericUoW.Repository<PestAlarmDetail>().GetAllByQuery(where.Compile(), join).ToList());
        }
        [HttpGet]
        public IActionResult PestAlarmDetail_Get(int id = 0)
        {
            var row = _genericUoW.Repository<PestAlarmDetail>().GetById(id);
            row.Alarm = _genericUoW.Repository<Alarm>().GetById(row.AlarmId);
            return PartialView(row);
        }
        [HttpPost]
        public async Task<JsonResult> PestAlarmDetail_Save(PestAlarmDetail model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
                       _genericUoW.Repository<PestAlarmDetail>().Update(model);
            return Json(_genericUoW.Save(user.Id, EnuAction.Update, "آلارم آفات"));
        }

        [HttpPost]
        public async Task<JsonResult> GetStation(int Projectid)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            else
            {
                List<long> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
                return Json(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x == n.Id) && n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            }
        }
        [HttpPost]
        public JsonResult GetAlarm(int Stationid)
        {
            return Json(_genericUoW.Repository<Alarm>().GetAll(n => n.StationId == Stationid && n.AlarmType == EnuAlarm.Pest).Select(n => new { n.Id, n.AlarmName }).OrderBy(m => m.AlarmName).ToList());
        }
    }
}