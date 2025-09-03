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
    [Authorize(Roles = "AdminiStratore,ForecastsAlarmDetail")]
    public class ForecastsAlarmDetailController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public ForecastsAlarmDetailController(GenericUoW genericUoW, UserManager<User> userManager)
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
                //ViewBag.AlarmList = new SelectList(_genericUoW.Repository<Alarm>().GetAll(n => n.AlarmType == Data.Enums.EnuAlarm.Forecasts).Select(n => new { n.Id, n.AlarmName }).ToList(), "Id", "AlarmName");
                return View();
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();

                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                ViewBag.StationList = new SelectList(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x.StationId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                //ViewBag.AlarmList = new SelectList(_genericUoW.Repository<Alarm>().GetAll(n => StationUserId.Any(x => x.StationId == n.StationId) && n.AlarmType == Data.Enums.EnuAlarm.Forecasts).Select(n => new { n.Id, n.AlarmName }).ToList(), "Id", "AlarmName");
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> ForecastsAlarmDetail_List(int ProjectId = 0, int StationId = 0, int AlarmId = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            IEnumerable<ForecastsAlarmDetail> model;
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                if (ProjectId != 0 && StationId != 0 && AlarmId != 0)
                {
                    List<long> AlarmIdList = _genericUoW.Repository<Alarm>().GetAllByQuery(n => n.Id == AlarmId && n.StationId == StationId).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<ForecastsAlarmDetail>().GetAllByQuery(n => AlarmIdList.Any(x => x == n.AlarmId), n => n.Alarm, x => x.ForecastsAlarmParameter);
                }
                else if (ProjectId != 0)
                {
                    List<long> StationWithProjectId = _genericUoW.Repository<Station>().GetAll(n => n.ProjectId == ProjectId).Select(n => n.Id).ToList();
                    List<long> AlarmIdList = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationWithProjectId.Any(x => x == n.StationId)).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<ForecastsAlarmDetail>().GetAllByQuery(n => AlarmIdList.Any(x => x == n.AlarmId), n => n.Alarm, x => x.ForecastsAlarmParameter);
                }
                else if (StationId != 0)
                {
                    List<long> AlarmIdList = _genericUoW.Repository<Alarm>().GetAllByQuery(n => n.StationId == StationId).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<ForecastsAlarmDetail>().GetAllByQuery(n => AlarmIdList.Any(x => x == n.AlarmId), n => n.Alarm, x => x.ForecastsAlarmParameter);
                }
                else if (AlarmId != 0)
                {
                    List<long> AlarmIdList = _genericUoW.Repository<Alarm>().GetAllByQuery(n => n.Id == AlarmId).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<ForecastsAlarmDetail>().GetAllByQuery(n => AlarmIdList.Any(x => x == n.AlarmId), n => n.Alarm);
                }
                else
                {
                    model = _genericUoW.Repository<ForecastsAlarmDetail>().GetAllByQuery(null, n => n.Alarm, x => x.ForecastsAlarmParameter);
                }
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                if (ProjectId != 0 && StationId != 0 && AlarmId != 0)
                {
                    List<long> AlarmIdList = _genericUoW.Repository<Alarm>().GetAllByQuery(n => n.Id == AlarmId && StationUserId.Any(x => x.StationId == n.StationId && n.StationId == StationId)).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<ForecastsAlarmDetail>().GetAllByQuery(n => AlarmIdList.Any(x => x == n.AlarmId), n => n.Alarm, x => x.ForecastsAlarmParameter);
                }
                else if (ProjectId != 0)
                {
                    List<long> StationWithProjectId = _genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x.StationId == n.Id) && n.ProjectId == ProjectId).Select(n => n.Id).ToList();
                    List<long> AlarmIdList = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationWithProjectId.Any(x => x == n.StationId)).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<ForecastsAlarmDetail>().GetAllByQuery(n => AlarmIdList.Any(x => x == n.AlarmId), n => n.Alarm, x => x.ForecastsAlarmParameter);
                }
                else if (StationId != 0)
                {
                    List<long> AlarmIdList = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationUserId.Any(x => x.StationId == n.StationId) && n.StationId == StationId).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<ForecastsAlarmDetail>().GetAllByQuery(n => AlarmIdList.Any(x => x == n.AlarmId), n => n.Alarm, x => x.ForecastsAlarmParameter);
                }
                else
                {
                    List<long> AlarmIdList = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationUserId.Any(x => x.StationId == n.StationId)).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<ForecastsAlarmDetail>().GetAllByQuery(n => AlarmIdList.Any(x => x == n.AlarmId), n => n.Alarm, x => x.ForecastsAlarmParameter);
                }
            }
            return PartialView(model.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> ForecastsAlarmDetail_Get(int id = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                //ViewBag.AlarmList = new SelectList(_genericUoW.Repository<Alarm>().GetAll(n => n.AlarmType == Data.Enums.EnuAlarm.Forecasts).Select(n => new { n.Id, n.AlarmName }).ToList(), "Id", "AlarmName");
                //ViewBag.ForecastsAlarmParameterList = new SelectList(_genericUoW.Repository<ForecastsAlarmParameter>().GetAll().Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                //ViewBag.AlarmList = new SelectList(_genericUoW.Repository<Alarm>().GetAll(n => StationUserId.Any(x => x.StationId == n.StationId) && n.AlarmType == Data.Enums.EnuAlarm.Forecasts).Select(n => new { n.Id, n.AlarmName }).ToList(), "Id", "AlarmName");
                ViewBag.ForecastsAlarmParameterList = new SelectList(_genericUoW.Repository<ForecastsAlarmParameter>().GetAll().Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            }
            return PartialView(id == 0 ? new ForecastsAlarmDetail() : _genericUoW.Repository<ForecastsAlarmDetail>().GetById(id));
        }
        [HttpPost]
        public async Task<JsonResult> ForecastsAlarmDetail_Save(ForecastsAlarmDetail model)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            if (model.Id == 0)
            {
                _genericUoW.Repository<ForecastsAlarmDetail>().Insert(model);
                return Json(_genericUoW.Save(user.Id, EnuAction.Update, "آلارم پیش بینی ها"));
            }
            else
            {
                _genericUoW.Repository<ForecastsAlarmDetail>().Update(model);
                return Json(_genericUoW.Save(user.Id, EnuAction.Update, "آلارم پیش بینی ها"));
            }
        }
        [HttpGet]
        public async Task<JsonResult> ForecastsAlarmDetail_Delete(int id)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<ForecastsAlarmDetail>().Delete(id);
            return Json(_genericUoW.Save(user.Id, EnuAction.Update, "آلارم پیش بینی ها"));
        }
        [HttpGet]
        public JsonResult ForecastsAlarmDetail_GetParameter(int id)
        {
            var row = _genericUoW.Repository<ForecastsAlarmParameter>().GetById(id);
            return Json(new { value = row.Value, icon = row.IconPath });
        }

    }
}