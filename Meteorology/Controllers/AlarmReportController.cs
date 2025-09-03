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
    [Authorize(Roles = "AdminiStratore,AlarmReport")]
    public class AlarmReportController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public AlarmReportController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager; 
            _genericUoW = genericUoW;
        }
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Expression<Func<Alarm, object>>[] join = { n => n.Station, n => n.Station.Project };
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                //ViewBag.StationList = new SelectList(_genericUoW.Repository<Station>().GetAll().Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
             //   ViewBag.AlarmList = new SelectList(_genericUoW.Repository<Alarm>().GetAll(n => n.AlarmType == Data.Enums.EnuAlarm.Pest).Select(n => new { n.Id, n.AlarmName }).ToList(), "Id", "AlarmName");
                return View();
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();

                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                //ViewBag.StationList = new SelectList(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x.StationId == n.Id)).Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
               // ViewBag.AlarmList = new SelectList(_genericUoW.Repository<Alarm>().GetAll(n => StationUserId.Any(x => x.StationId == n.StationId) && n.AlarmType == Data.Enums.EnuAlarm.Pest).Select(n => new { n.Id, n.AlarmName }).ToList(), "Id", "AlarmName");
                return View();
            }
        }
        [HttpGet] 
        public async Task<IActionResult> AlarmReport_List(int ProjectId = 0, int StationId = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Expression<Func<Alarm, object>>[] join = { n =>  n.Station, n => n.Station.Project };
            Expression<Func<Alarm, bool>> where = null;

            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                if (ProjectId != 0 && StationId != 0 )
                {
                    where = n => n.Station.ProjectId == ProjectId && n.Station.Id == StationId;
                }
                else if (ProjectId != 0)
                {
                    where = n => n.Station.ProjectId == ProjectId;
                }
                else if (StationId != 0)
                {
                    where = n => n.Station.Id == StationId;
                } 
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                if (ProjectId != 0 && StationId != 0 )
                {
                    where = n => StationUserId.Any(x => x.ProjectId == ProjectId && x.StationId == StationId) && n.Station.ProjectId == ProjectId  && n.Station.Id == StationId;
                }
                else if (ProjectId != 0)
                {
                    where = n => StationUserId.Any(x => x.ProjectId == ProjectId) && n.Station.ProjectId == ProjectId;
                }
                else if (StationId != 0)
                {
                    where = n => StationUserId.Any(x => x.StationId == StationId) && n.Station.Id == StationId;
                }
                else
                {
                    where = n => StationUserId.Any(x => x.StationId == StationId);
                }
            }
            return PartialView(_genericUoW.Repository<Alarm>().GetAllByQuery(where.Compile(), join).ToList());
        }
        [HttpGet]
        public IActionResult PestAlarmLog_Get(int id = 0)
        {
            PestAlarmDetail row = _genericUoW.Repository<PestAlarmDetail>().GetAllByQuery(p => p.AlarmId == id).FirstOrDefault();
                        return PartialView(row);
        }
        public IActionResult SensorAlarmLog_Get(int id = 0)
        {
            List<AlarmLog> row = _genericUoW.Repository<AlarmLog>().GetAllByQuery(p => p.AlarmId == id).ToList();
            return PartialView(row);
        }
        [HttpGet]
        public IActionResult SmsSendLog_Get(int id = 0)
        {
           List<SmsSend> row = _genericUoW.Repository<SmsSend>().GetAllByQuery().ToList();
            return PartialView(row);
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