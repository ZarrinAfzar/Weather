using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,PestAlarmReport")]
    public class PestAlarmReportController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public PestAlarmReportController(GenericUoW genericUoW, UserManager<User> userManager)
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
                ViewBag.StationList = new SelectList(_genericUoW.Repository<Station>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                ViewBag.AlarmList = new SelectList(_genericUoW.Repository<Alarm>().GetAll(n => n.AlarmType == Data.Enums.EnuAlarm.Pest).Select(n => new { n.Id, n.AlarmName }).OrderBy(m => m.AlarmName).ToList(), "Id", "AlarmName");
                return View();
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();

                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                ViewBag.StationList = new SelectList(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x.StationId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                ViewBag.AlarmList = new SelectList(_genericUoW.Repository<Alarm>().GetAll(n => StationUserId.Any(x => x.StationId == n.StationId) && n.AlarmType == Data.Enums.EnuAlarm.Pest).OrderBy(m => m.AlarmName).Select(n => new { n.Id, n.AlarmName }).ToList(), "Id", "AlarmName");
                return View();
            }
        }
        [HttpGet] 
        public async Task<IActionResult> PestAlarmReport_List(int ProjectId = 0, int StationId = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Expression<Func<PestAlarmDetail, object>>[] join = { n => n.Alarm, n => n.Alarm.Station, n => n.Alarm.Station.Project };
            Expression<Func<PestAlarmDetail, bool>> where = null;

            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                if (ProjectId != 0 && StationId != 0 )
                {
                    where = n => n.Alarm.Station.ProjectId == ProjectId && n.Alarm.Station.Id == StationId;
                }
                else if (ProjectId != 0)
                {
                    where = n => n.Alarm.Station.ProjectId == ProjectId;
                }
                else if (StationId != 0)
                {
                    where = n => n.Alarm.Station.Id == StationId;
                } 
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                if (ProjectId != 0 && StationId != 0 )
                {
                    where = n => StationUserId.Any(x => x.ProjectId == ProjectId && x.StationId == StationId) && n.Alarm.Station.ProjectId == ProjectId  && n.Alarm.Station.Id == StationId;
                }
                else if (ProjectId != 0)
                {
                    where = n => StationUserId.Any(x => x.ProjectId == ProjectId) && n.Alarm.Station.ProjectId == ProjectId;
                }
                else if (StationId != 0)
                {
                    where = n => StationUserId.Any(x => x.StationId == StationId) && n.Alarm.Station.Id == StationId;
                } 
            }
            return PartialView(_genericUoW.Repository<PestAlarmDetail>().GetAllByQuery(where.Compile(), join).ToList());
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
    }
}