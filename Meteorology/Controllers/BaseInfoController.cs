using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    [Authorize(Roles = "AdminiStratore,BaseInfo")]
    public class BaseInfoController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _environment;

        public BaseInfoController(GenericUoW genericUoW, UserManager<User> userManager, IHostingEnvironment environment)
        {
            _genericUoW = genericUoW;
            _userManager = userManager;
            _environment = environment;
        }
        public IActionResult Index()
        {
            BaseInfoVm model = new BaseInfoVm()
            {
                States = _genericUoW.Repository<State>().GetAll().ToList(),
                StationTypes = _genericUoW.Repository<StationType>().GetAll().ToList(),
                Cities = _genericUoW.Repository<City>().GetAll(null,n=>n.State).ToList(),
                DataLoggers = _genericUoW.Repository<DataLogger>().GetAll().ToList(),
                ModemTypes = _genericUoW.Repository<ModemType>().GetAll().ToList(),
                SensorTypes = _genericUoW.Repository<SensorType>().GetAll().ToList(),
                ForecastsAlarmParameters = _genericUoW.Repository<ForecastsAlarmParameter>().GetAll().ToList(),
                Units = _genericUoW.Repository<Unit>().GetAll().ToList(),
                VirtualSensorBases = _genericUoW.Repository<VirtualSensorBase>().GetAll(null,n=>n.SensorType).ToList()
            };
            ViewBag.StateList = new SelectList(model.States, "Id", "Name");
            //List<long> st =  model.VirtualSensorBases.Select(n=>n.SensorTypeId).ToList();
            ViewBag.SensorList = new SelectList(model.SensorTypes.Where(n => n.SensorType_State == EnuSensorType.Virtual).ToList(), "Id", "FaName");// && st.Any(x => x != n.Id)
            return View(model);
        }




        #region State
        [HttpPost]
        public async Task<JsonResult> State_Save(string id, string name)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            State model = new State()
            { 
                Id = Convert.ToInt32(id),
                Name = name
            };
            if (model.Id == 0)
            {
                _genericUoW.Repository<State>().Insert(model);
                return _genericUoW.Save(user.Id,EnuAction.Create, "استان") ? Json(model) : Json(false);
            }
            else
            {
                _genericUoW.Repository<State>().Update(model);
                return _genericUoW.Save(user.Id,EnuAction.Update,"استان") ? Json(model) : Json(false);
            }
        }
        [HttpGet]
        public JsonResult State_Get(int id)
        {
            return Json(_genericUoW.Repository<State>().GetById(id));
        }
        [HttpGet]
        public async Task<JsonResult> State_Delete(int id)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<State>().Delete(id);
            return _genericUoW.Save(user.Id, EnuAction.Delete, "استان") ? Json(id) : Json(false);
        }
        [HttpGet]
        public IActionResult State_UpdateCombo()
        {
            return Json(_genericUoW.Repository<State>().GetAll().Select(n => new { n.Id, n.Name }).ToList());
        }
        #endregion

        #region City
        [HttpPost]
        public async Task<JsonResult> City_Save(int id,string name,int stateId)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            City model = new City()
            {
                Id = id,
                StateId = stateId,
                Name = name,
            };
            if (model.Id == 0)
            {
                _genericUoW.Repository<City>().Insert(model);
                return _genericUoW.Save(user.Id, EnuAction.Create, "شهرستان") ? Json(model) : Json(false);
            }
            else
            {
                _genericUoW.Repository<City>().Update(model);
                return _genericUoW.Save(user.Id, EnuAction.Update, "شهرستان") ? Json(model) : Json(false);
            }
        }
        [HttpGet]
        public JsonResult City_Get(int id)
        {
            return Json(_genericUoW.Repository<City>().GetById(id));
        }
        [HttpGet]
        public async Task<JsonResult> City_Delete(int id) 
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<City>().Delete(id);
            return _genericUoW.Save(user.Id, EnuAction.Delete, "شهرستان") ? Json(id) : Json(false);
        }
        #endregion

        #region DataLogger
        [HttpPost]
        public async Task<JsonResult> DataLogger_Save(int id, string name)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            DataLogger model = new DataLogger()
            {
                Id = id,
                Name = name,
            };
            if (model.Id == 0)
            {
                _genericUoW.Repository<DataLogger>().Insert(model);
                return _genericUoW.Save(user.Id, EnuAction.Create, "دیتالاگر") ? Json(model) : Json(false);
            }
            else
            {
                _genericUoW.Repository<DataLogger>().Update(model);
                return _genericUoW.Save(user.Id, EnuAction.Update, "دیتالاگر") ? Json(model) : Json(false);
            }
        }
        [HttpGet]
        public JsonResult DataLogger_Get(int id)
        {
            return Json(_genericUoW.Repository<DataLogger>().GetById(id));
        }
        [HttpGet]
        public async Task<JsonResult> DataLogger_Delete(int id)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<DataLogger>().Delete(id);
            return _genericUoW.Save(user.Id, EnuAction.Delete, "دیتالاگر") ? Json(id) : Json(false);
        }
        #endregion

        #region ModemType
        [HttpPost]
        public async Task<JsonResult> ModemType_Save(int id, string name)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            ModemType model = new ModemType()
            {
                Id = id,
                Name = name, 
            };
            if (model.Id == 0)
            { 
                _genericUoW.Repository<ModemType>().Insert(model);
                return _genericUoW.Save(user.Id, EnuAction.Create, "انواع مودم") ? Json(model) : Json(false);
            }
            else
            {
                _genericUoW.Repository<ModemType>().Update(model);
                return _genericUoW.Save(user.Id, EnuAction.Update, "انواع مودم") ? Json(model) : Json(false);
            }
        }
        [HttpGet]
        public JsonResult ModemType_Get(int id)
        {
            return Json(_genericUoW.Repository<ModemType>().GetById(id));
        }
        [HttpGet]
        public async Task<JsonResult> ModemType_Delete(int id)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<ModemType>().Delete(id);
            return _genericUoW.Save(user.Id, EnuAction.Delete, "انواع مودم") ? Json(id) : Json(false); 
        }
        #endregion

        #region StationType
        [HttpPost]
        public async Task<JsonResult> StationType_Save(IFormFile Icon, StationType model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            //model.Icon = null;
            //if (Icon != null && Icon.Length > 0)
            //{
            //    var webRoot = _environment.WebRootPath;
            //    if (!Directory.Exists(webRoot + "/StationTypeImage/"))
            //    {
            //        Directory.CreateDirectory(webRoot + "/StationTypeImage/");
            //    }
            //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/StationTypeImage/", Icon.FileName);
            //    using (var stream = new FileStream(path, FileMode.Create))
            //    {
            //        await Icon.CopyToAsync(stream);
            //    }
            //    model.Icon = Icon.FileName;
            //}

            //if (model.Id == 0)
            //{
            //    _genericUoW.Repository<StationType>().Insert(model);
            //    return _genericUoW.Save(user.Id, EnuAction.Create, "انواع ایستگاه") ? Json(model) : Json(false);
            //}
            //else
            //{
            //    var old = _genericUoW.Repository<StationType>().GetById(model.Id);
            //    model.Icon = Icon == null ? old.Icon : Icon.FileName;
            //    _genericUoW.Repository<StationType>().Update(model, old);
                return _genericUoW.Save(user.Id, EnuAction.Update, "انواع ایستگاه") ? Json(model) : Json(false);
           // }
        }
        [HttpGet]
        public JsonResult StationType_Get(int id)
        {
            return Json(_genericUoW.Repository<StationType>().GetById(id));
        }
        [HttpGet]
        public async Task<JsonResult> StationType_Delete(int id)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<StationType>().Delete(id);
            return _genericUoW.Save(user.Id, EnuAction.Delete, "انواع ایستگاه") ? Json(id) : Json(false);
        }

        #endregion

        #region SensorType
        [HttpPost]
        public async Task<JsonResult> SensorTypes_Save(SensorType model)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            if (model.Id == 0)
            {
                _genericUoW.Repository<SensorType>().Insert(model);
                return _genericUoW.Save(user.Id, EnuAction.Create, "انواع سنسور") ? Json(model) : Json(false);
            }
            else
            {
                _genericUoW.Repository<SensorType>().Update(model);
                return _genericUoW.Save(user.Id, EnuAction.Update, "انواع سنسور") ? Json(model) : Json(false);
            }
        }
        [HttpGet]
        public JsonResult SensorTypes_Get(int id)
        {
            return Json(_genericUoW.Repository<SensorType>().GetById(id));
        }
        [HttpGet]
        public async Task<JsonResult> SensorTypes_Delete(int id)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<SensorType>().Delete(id);
            return _genericUoW.Save(user.Id, EnuAction.Delete, "انواع سنسور") ? Json(id) : Json(false);
        }
        [HttpGet]
        public IActionResult SensorTypes_UpdateCombo()
        {
            List<long> notSet = _genericUoW.Repository<VirtualSensorBase>().GetAll().Select(n => n.SensorTypeId).ToList();
            return Json(_genericUoW.Repository<SensorType>().GetAll(n=>n.SensorType_State == EnuSensorType.Virtual && notSet.Any(x=> x != n.Id)).Select(n => new { n.Id, n.FaName }).ToList());
        }
        #endregion

        #region ForecastsAlarmParameter
        [HttpPost]
        public async Task<JsonResult> ForecastsAlarmParameter_Save(IFormFile Icon, ForecastsAlarmParameter model)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            model.Icon = null;
            if (Icon != null && Icon.Length > 0)
            {
                var webRoot = _environment.WebRootPath;
                if (!Directory.Exists(webRoot + "/ForecastsAlarmParameterImage/"))
                {
                    Directory.CreateDirectory(webRoot + "/ForecastsAlarmParameterImage/");
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ForecastsAlarmParameterImage/", Icon.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Icon.CopyToAsync(stream);
                }
                model.Icon = Icon.FileName;
            }

            if (model.Id == 0)
            {
                _genericUoW.Repository<ForecastsAlarmParameter>().Insert(model);
                return _genericUoW.Save(user.Id, EnuAction.Create, "پارامتر آلارم افات") ? Json(model) : Json(false);
            }
            else
            {
                var old = _genericUoW.Repository<ForecastsAlarmParameter>().GetById(model.Id);
                model.Icon = Icon == null ? old.Icon : Icon.FileName;
                _genericUoW.Repository<ForecastsAlarmParameter>().Update(model, old);
                return _genericUoW.Save(user.Id, EnuAction.Update, "پارامتر آلارم افات") ? Json(model) : Json(false);
            }
        }
        [HttpGet]
        public JsonResult ForecastsAlarmParameter_Get(int id)
        {
            return Json(_genericUoW.Repository<ForecastsAlarmParameter>().GetById(id));
        }
        [HttpGet]
        public async Task<JsonResult> ForecastsAlarmParameter_Delete(int id)
        {
            User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<ForecastsAlarmParameter>().Delete(id);
            return _genericUoW.Save(user.Id, EnuAction.Delete, "پارامتر آلارم افات") ? Json(id) : Json(false);
        }
        #endregion

        #region Unit
        [HttpPost]
        public async Task<JsonResult> Unit_Save(string id, string FaName, string EnName)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            Unit model = new Unit()
            {
                Id = Convert.ToInt32(id),
                FaName = FaName,
                EnName = EnName, 
            };
            if (model.Id == 0)
            {
                _genericUoW.Repository<Unit>().Insert(model);
                return _genericUoW.Save(user.Id, EnuAction.Create, "واحد اندازه گیری") ? Json(model) : Json(false);
            }
            else
            {
                _genericUoW.Repository<Unit>().Update(model);
                return _genericUoW.Save(user.Id, EnuAction.Update, "واحد اندازه گیری") ? Json(model) : Json(false);
            }
        }
        [HttpGet]
        public JsonResult Unit_Get(int id)
        {
            return Json(_genericUoW.Repository<Unit>().GetById(id));
        }
        [HttpGet]
        public async Task<JsonResult> Unit_Delete(int id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<Unit>().Delete(id);
            return _genericUoW.Save(user.Id, EnuAction.Delete, "واحد اندازه گیری") ? Json(id) : Json(false);
        }

        #endregion
         
        #region VirtualSensorBase
        [HttpPost]
        public async Task<JsonResult> VirtualSensorBase_Save(VirtualSensorBase model)
        { 
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (model.Id == 0)
            {
                _genericUoW.Repository<VirtualSensorBase>().Insert(model);
                return _genericUoW.Save(user.Id, EnuAction.Create, "اطلاعات پایه پارامتر محاسباتی") ? Json(model) : Json(false);
            }
            else
            {
                _genericUoW.Repository<VirtualSensorBase>().Update(model); 
                return _genericUoW.Save(user.Id, EnuAction.Update, "اطلاعات پایه پارامتر محاسباتی") ? Json(model) : Json(false);
            }
        }
        [HttpGet]
        public JsonResult VirtualSensorBase_Get(int id)
        {
            return Json(_genericUoW.Repository<VirtualSensorBase>().GetById(id));
        }
        [HttpGet]
        public async Task<JsonResult> VirtualSensorBase_Delete(int id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<VirtualSensorBase>().Delete(id);
            return _genericUoW.Save(user.Id, EnuAction.Delete, "اطلاعات پایه پارامتر محاسباتی") ? Json(id) : Json(false);
        }

        #endregion
    }
}