using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,VirtualSensorDetail")]
    public class VirtualSensorDetailController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public VirtualSensorDetailController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            SelectList ProjectList;
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            }
            ViewBag.ProjectList = ProjectList;
            return View();
        }

        #region DRP
        [HttpPost]
        public async Task<JsonResult> GetStation(int projectId)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == projectId).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == projectId && StationUserId.Any(x => x.StationId == n.Id)).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            }
        }
        [HttpPost]
        public JsonResult GetVirtualDetail(int stationId)
        {
            return Json(_genericUoW.Repository<VirtualSensorDetail>().GetAll(n => n.StationId == stationId).Select(n => new { n.Id, n.Title }).ToList());
        }
        [HttpGet]
        public IActionResult VirtualSensorBase_List()  
        {
            return PartialView(_genericUoW.Repository<VirtualSensorBase>().GetAll(null,n=>n.SensorType));
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> VirtualSensorDetail_GetNew(int Id)
        {
            ViewBag.UnitList = new SelectList(_genericUoW.Repository<Unit>().GetAll().Select(n => new { n.Id, n.FaName }).ToList(), "Id", "FaName");
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
            return PartialView(_genericUoW.Repository<VirtualSensorBase>().GetById(Id));
        }
        [HttpPost]
        public async Task<JsonResult> VirtualSensorDetail_GetNew_Save(IFormCollection form)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var stations = _genericUoW.Repository<SensorSetting>().GetAll(n => n.StationId == Convert.ToInt32(form["StationId"])).ToList();
            SensorSetting sensorSetting = new SensorSetting();
            sensorSetting.StationId = Convert.ToInt32(form["StationId"]);
            sensorSetting.SensorEnable = true;
            sensorSetting.SensorType = 2;
            sensorSetting.SensorRow = stations.Count() + 1;
            sensorSetting.SensorTypeId = Convert.ToInt32(form["SensorTypeId"]);
            sensorSetting.SensorName = form["Title"];
            sensorSetting.UnitId = Convert.ToInt32(form["Unit"]);
            sensorSetting.SensorDigit = Convert.ToInt32(form["Digit"]);
            sensorSetting.SensorDateTime = DateTime.Now;
            _genericUoW.Repository<SensorSetting>().Insert(sensorSetting);

            VirtualSensorDetail virtualSensorDetail = new VirtualSensorDetail()
            {
                StationId = sensorSetting.StationId,
                Title = form["Title"],
                ParameterName1 = form["ParameterName1"],
                ParameterName2 = form["ParameterName2"],
                ParameterName3 = form["ParameterName3"],
                ParameterName4 = form["ParameterName4"],
                ParameterName5 = form["ParameterName5"],
                ParameterType1 = form["ParameterType1"],
                ParameterType2 = form["ParameterType2"],
                ParameterType3 = form["ParameterType3"],
                ParameterType4 = form["ParameterType4"],
                ParameterType5 = form["ParameterType5"],
                ParameterValue1 = form["ParameterValue1"],
                ParameterValue2 = form["ParameterValue2"],
                ParameterValue3 = form["ParameterValue3"],
                ParameterValue4 = form["ParameterValue4"],
                ParameterValue5 = form["ParameterValue5"],
            };
            _genericUoW.Repository<VirtualSensorDetail>().Insert(virtualSensorDetail);
            return Json(_genericUoW.Save(user.Id, Data.Enums.EnuAction.Create, "پارامتر محاسباتی"));
        }



        [HttpGet]
        public IActionResult VirtualSensorDetail_GetEdit(int Id) 
        {
            ViewBag.UnitList = new SelectList(_genericUoW.Repository<Unit>().GetAll().Select(n => new { n.Id, n.FaName }).ToList(), "Id", "FaName");
            return PartialView(_genericUoW.Repository<VirtualSensorDetail>().GetById(Id));
        }
        [HttpPost]
        public async Task<JsonResult> VirtualSensorDetail_GetEdit_Save(IFormCollection form)
        {
            VirtualSensorDetail model = new VirtualSensorDetail()
            {
                Id = Convert.ToInt64(form["Id"]),
                StationId = Convert.ToInt64(form["StationId"]),
                Title = form["Title"],
                ParameterName1 = form["ParameterName1"],
                ParameterName2 = form["ParameterName2"],
                ParameterName3 = form["ParameterName3"],
                ParameterName4 = form["ParameterName4"],
                ParameterName5 = form["ParameterName5"],
                ParameterType1 = form["ParameterType1"],
                ParameterType2 = form["ParameterType2"],
                ParameterType3 = form["ParameterType3"],
                ParameterType4 = form["ParameterType4"],
                ParameterType5 = form["ParameterType5"],
                ParameterValue1 = form["ParameterValue1"],
                ParameterValue2 = form["ParameterValue2"],
                ParameterValue3 = form["ParameterValue3"],
                ParameterValue4 = form["ParameterValue4"],
                ParameterValue5 = form["ParameterValue5"],
            };
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<VirtualSensorDetail>().Update(model);
            return Json(_genericUoW.Save(user.Id, Data.Enums.EnuAction.Update, "پارامتر محاسباتی"));
        }
    }
}