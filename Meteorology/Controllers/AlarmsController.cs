using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,Alarms")]
    public class AlarmsController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public AlarmsController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int count = _genericUoW.Repository<Alarm>().GetAll().ToList().Count;
            ViewBag.pagecount = (int)Math.Ceiling((double)count / 6);

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


        #region Alarms
        [HttpGet]
        public async Task<IActionResult> Alarms_List(int ProjectId = 0, int StationId = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                List<Alarm> model = new List<Alarm>();
                if (ProjectId != 0 && StationId != 0)
                {
                    List<long> StationWithProjectId = _genericUoW.Repository<Station>().GetAll(n => n.ProjectId == ProjectId).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationWithProjectId.Any(x => x == n.StationId) && n.StationId == StationId, n => n.Station, x => x.AlarmTells).ToList();
                }
                else if (ProjectId != 0)
                {
                    List<long> StationWithProjectId = _genericUoW.Repository<Station>().GetAll(n => n.ProjectId == ProjectId).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationWithProjectId.Any(x => x == n.StationId), n => n.Station, x => x.AlarmTells).ToList();
                }
                else if (StationId != 0)
                {
                    model = _genericUoW.Repository<Alarm>().GetAllByQuery(n => n.StationId == StationId, n => n.Station, x => x.AlarmTells).ToList();
                }
                else
                {
                    model = _genericUoW.Repository<Alarm>().GetAllByQuery(null, n => n.Station, x => x.AlarmTells).ToList();
                }
                model = model.OrderBy(m => m.Station.Project.Name).ThenBy(n=>n.Station.Name).ToList();
                return PartialView(model);
            }
            else
            {
                List<Alarm> model = new List<Alarm>();
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                if (ProjectId != 0 && StationId != 0)
                {
                    List<long> StationWithProjectId = _genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.ProjectId) && n.ProjectId == ProjectId).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationWithProjectId.Any(x => x == n.StationId) && StationUserId.Any(x => x.StationId == n.StationId) && n.StationId == StationId, n => n.Station, x => x.AlarmTells).ToList();
                }
                else if (ProjectId != 0)
                {
                    List<long> StationWithProjectId = _genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.ProjectId) && n.ProjectId == ProjectId).Select(n => n.Id).ToList();
                    model = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationWithProjectId.Any(x => x == n.StationId) && StationUserId.Any(x => x.StationId == n.StationId), n => n.Station, x => x.AlarmTells).ToList();
                }
                else if (StationId != 0)
                {
                    model = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationUserId.Any(x => x.StationId == n.StationId), n => n.Station, x => x.AlarmTells).ToList();
                }
                else
                {
                    model = _genericUoW.Repository<Alarm>().GetAllByQuery(n => StationUserId.Any(x => x.StationId == n.StationId), n => n.Station, x => x.AlarmTells).ToList();
                }
                model = model.OrderBy(m => m.Station.Project.Name).ThenBy(n => n.Station.Name).ToList();
                return PartialView(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Alarms_Get(int id)
        {
            Alarm model = id==0? new Alarm() : _genericUoW.Repository<Alarm>().GetAllByQuery(a => a.Id == id, n => n.Station).First();
            if (id > 0)
            {

                ViewBag.StationList = new SelectList(_genericUoW.Repository<Station>().GetAll().Where(a=>a.ProjectId==model.Station.ProjectId).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            }
            
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");

            }
           

            return PartialView(model);
        }
        [HttpPost]
        public async Task<JsonResult> Alarms_Save([FromBody] Alarm model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (model.Id == 0)
            {
                _genericUoW.Repository<Alarm>().Insert(model);
                if (_genericUoW.Save(user.Id, Data.Enums.EnuAction.Create, "آلارم"))
                {
                    if (model.AlarmType == Data.Enums.EnuAlarm.Pest)
                    {
                        PestAlarmDetail pestAlarmDetail = new PestAlarmDetail()
                        {
                            AlarmId = model.Id,
                            CountLevel=0,
                            LevelNow = 1
                        };
                        _genericUoW.Repository<PestAlarmDetail>().Insert(pestAlarmDetail);
                        return Json(_genericUoW.Save(user.Id, Data.Enums.EnuAction.Create, "آلارم "));
                    }
                    return Json(true);
                }
                return Json(false);
            }
            else
            {
                _genericUoW.Repository<Alarm>().Update(model);
                return Json(_genericUoW.Save(user.Id, Data.Enums.EnuAction.Update, "آلارم"));
            }
        }
        [HttpGet]
        public async Task<JsonResult> Alarms_Delete(int id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<Alarm>().Delete(id);
            return Json(_genericUoW.Save(user.Id, Data.Enums.EnuAction.Delete, "آلارم"));
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
        #endregion

        #region Alarms_Station
        [HttpGet]
        public IActionResult Alarms_Station(int id)
        {
            var model = _genericUoW.Repository<Station>().GetById(id);
            model.Project = _genericUoW.Repository<Project>().GetById(model.ProjectId);
            model.StationType = _genericUoW.Repository<StationType>().GetById(model.StationTypeId);
            model.ModemType = _genericUoW.Repository<ModemType>().GetById(model.ModemTypeId);
            model.DataLogger = _genericUoW.Repository<DataLogger>().GetById(model.DataLoggerId);
            return PartialView(model);
        }
        #endregion

        #region AlarmsTell
        [HttpGet]
        public IActionResult AlarmTell_List(int AlarmId,int StationId)
        {
            AlarmTellViewModel model = new AlarmTellViewModel()
            {
                AlarmId = AlarmId
            };
            model.AlarmTells = _genericUoW.Repository<AlarmTell>().GetAllByQuery(n => n.AlarmId == AlarmId).ToList();

            if (model.AlarmTells.Count() > 0)
                model.StationTels = _genericUoW.Repository<StationTel>().GetAllByQuery(n => !model.AlarmTells.Select(x => x.StationTelId).Contains(n.Id) && n.StationId == StationId).OrderBy(m => new { m.LastName, m.Name }).ToList();//
            else
                model.StationTels = _genericUoW.Repository<StationTel>().GetAllByQuery(n=>n.StationId== StationId).OrderBy(m => new { m.LastName, m.Name }).ToList();//



            return PartialView("Alarms_Tell", model);
        }
        [HttpGet]
        public JsonResult AlarmTell_Get(int id)
        {
            return Json(_genericUoW.Repository<AlarmTell>().GetAll(t=>t.Id==id,n=>n.StationTel));
        }
        [HttpGet]
        public async Task<JsonResult> AlarmTell_Add(int StationTellid, int AlarmId)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
                       AlarmTell model = new AlarmTell()
            {
                AlarmId = AlarmId,
                StationTelId = StationTellid,
               
            };
            _genericUoW.Repository<AlarmTell>().Insert(model);
            _genericUoW.Save(user.Id, Data.Enums.EnuAction.Create, "تلفن های آلارم");
            return Json(model.Id);
        }
        [HttpGet]
        public async Task<JsonResult> AlarmTell_Delete(int Id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            AlarmTell stId = _genericUoW.Repository<AlarmTell>().GetById(Id);
            _genericUoW.Repository<AlarmTell>().Delete(stId);
            return Convert.ToBoolean(_genericUoW.Save(user.Id, Data.Enums.EnuAction.Delete, "آلارم")) ? Json(stId.StationTelId) : Json(false);
        }

        //[HttpPost]
        //public async Task<JsonResult> AlarmTell_Save(int id, int AlarmId, int StationId)
        //{
        //    User user = await _userManager.GetUserAsync(HttpContext.User);
        //    var model = new AlarmTell()
        //    {
        //        Id = id,
        //        AlarmId = AlarmId,
        //        StationId = StationId,
        //    };
        //    if (model.Id == 0)
        //    {
        //        _genericUoW.Repository<AlarmTell>().Insert(model);
        //        return Convert.ToBoolean(_genericUoW.Save(user.Id, Data.Enums.EnuAction.Create, "آلارم")) ? Json(model) : Json(false);
        //    }
        //    else
        //    {
        //        _genericUoW.Repository<AlarmTell>().Update(model);
        //        return Convert.ToBoolean(_genericUoW.Save(user.Id, Data.Enums.EnuAction.Update, "آلارم")) ? Json(model) : Json(false);
        //    }
        //}

        #endregion

    }
}