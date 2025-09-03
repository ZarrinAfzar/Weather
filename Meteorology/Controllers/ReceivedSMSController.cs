using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weather.Data.Base;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Data.SqlTypes;
using Weather.Data.Enums;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,ReceivedSMS")]
    public class ReceivedSMSController : Controller
    {

        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public ReceivedSMSController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
        }
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).OrderBy(m => m.Name).ToList(), "Id", "Name");
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
        public async Task<JsonResult> ReceivedSMS_Delete(int id)
        {
            if (id != 0)
            {
                User user = await _userManager.GetUserAsync(HttpContext.User);
                //_genericUoW.Repository<ReceivedSMS>().Delete(id);
                //return Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Delete, "ایستگاه")));
            }
            return Json(false);
        }
        [HttpGet]
        public IActionResult ReceivedSMS_EditValue(int id)
        {
            var model = new ReceivedSMS();
            model = _genericUoW.Repository<ReceivedSMS>().GetById(id);
            return PartialView("ReceivedSMS_EditValue", model);
        }
        [HttpPost]
        public async Task<JsonResult> ReceivedSMS_Save(ReceivedSMS modelold)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var model = new ReceivedSMS();  
            model = _genericUoW.Repository<ReceivedSMS>().GetById(modelold.Id);
            model.Value = modelold.Value;
            _genericUoW.Repository<ReceivedSMS>().Update(model, modelold);
            return Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Update, "پیامک ایستگاه")) ? Json(model) : Json(false);


        }
        [HttpGet]
        public async Task<JsonResult> ReceivedSMS_Activated(int id, bool Activated = false)
        {
            if (id != 0)
            {
                User user = await _userManager.GetUserAsync(HttpContext.User);
                ReceivedSMS Receivedsms = _genericUoW.Repository<ReceivedSMS>().GetById(id);

                Receivedsms.Accepted = Activated;

                _genericUoW.Repository<ReceivedSMS>().Update(Receivedsms);

                return Json(_genericUoW.Save(0, EnuAction.Update, ""));

            }
            else
                return Json(false);
        }
        public async Task<JsonResult> ReceivedSMS_EditValue(int id, double Value = 0)
        {
            if (id != 0)
            {
                User user = await _userManager.GetUserAsync(HttpContext.User);
                ReceivedSMS Receivedsms = _genericUoW.Repository<ReceivedSMS>().GetById(id);

                Receivedsms.Value = Value;

                _genericUoW.Repository<ReceivedSMS>().Update(Receivedsms);

                return Json(_genericUoW.Save(0, EnuAction.Update, ""));

            }
            else
                return Json(false);
        }
        public IActionResult ReceivedSMS_List(int stationId, string startDate, string endDate, string type)
        {
            List<ReceivedSMS> ReceivedSMS = new List<ReceivedSMS>();
            try
            {
                if (type == "1") type = "S";
                if (type == "2") type = "E";
                if (type == "3") type = "M";
                if (type == "4") type = "F";
                if (type == "5") type = "O";
                if (type == "6") type = "P";

                DateTime stDate = startDate.ToGreDateTime();
                DateTime enDate = endDate.ToGreDateTime();


                if (stationId > 0)
                    if (type != "0")
                        ReceivedSMS = _genericUoW.Repository<ReceivedSMS>().GetAllByQuery(n => n.StationId == stationId && n.ReceivedTime >= stDate && n.ReceivedTime <= enDate && n.Type == type).OrderByDescending(n => n.ReceivedTime).ToList();
                    else
                        ReceivedSMS = _genericUoW.Repository<ReceivedSMS>().GetAllByQuery(n => n.StationId == stationId && n.ReceivedTime >= stDate && n.ReceivedTime <= enDate).OrderByDescending(n => n.ReceivedTime).ToList();

                else
                     if (type != "0")
                    ReceivedSMS = _genericUoW.Repository<ReceivedSMS>().GetAllByQuery(n => n.ReceivedTime >= stDate.Date && n.ReceivedTime <= enDate.Date && n.Type == type).OrderByDescending(n => n.ReceivedTime).ToList();
                else
                    ReceivedSMS = _genericUoW.Repository<ReceivedSMS>().GetAllByQuery(n => n.ReceivedTime >= stDate.Date && n.ReceivedTime <= enDate.Date).OrderByDescending(n => n.ReceivedTime).ToList();

                return PartialView(ReceivedSMS);

                //string[,] Parametr = new string[4, 2];
                //Parametr[0, 0] = "@FromDate";
                //Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
                //Parametr[1, 0] = "@ToDate";
                //Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
                //Parametr[2, 0] = "@StationId";
                //Parametr[2, 1] = stationId.ToString();
                //Parametr[3, 0] = "@Type";
                //Parametr[3, 1] = type;


                //DataTable model = _genericUoW.GetFromSp(Parametr, "Sp_ReceivedSMS");

                // return PartialView(model);
            }
            catch (Exception ex)
            {

                return (new EmptyResult());
            }
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


        [HttpGet]
        public IActionResult ExportToExcel(int stationId, string stationname, string startDate, string endDate, string type)
        {
            string export = stationname + "_" + DateTime.Now.ToPeString();
            DateTime stDate = startDate.ToGreDateTime();
            DateTime enDate = endDate.ToGreDateTime();

            string[,] Parametr = new string[4, 2];
            Parametr[0, 0] = "@FromDate";
            Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[1, 0] = "@ToDate";
            Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[2, 0] = "@StationId";
            Parametr[2, 1] = stationId.ToString();
            Parametr[3, 0] = "@Type";
            Parametr[3, 1] = type;

            DataTable dt = _genericUoW.GetFromSp(Parametr, "Sp_ReceivedSMS");

            byte[] fileContents;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(export);
                worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                fileContents = package.GetAsByteArray();
            }
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: export + ".xlsx"
            );
        }
    }
}