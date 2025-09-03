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

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,DataErorrReport")]
    public class DataErorrReportController : Controller
    {

        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public DataErorrReportController(GenericUoW genericUoW, UserManager<User> userManager)
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
        public IActionResult DataErorrReport_List(string startDate, string endDate)
        {
            //ViewBag.Max = max;
            //ViewBag.Min = min;
            DataTable dt;
            try
            {
                DateTime stDate = startDate.ToGreDateTime();
                DateTime enDate = endDate.ToGreDateTime();
                List<double> data = new List<double>();
                List<string> RangeDate = new List<string>();
                string[,] Parametr = new string[2, 2];
                Parametr[0, 0] = "@FromDate";
                Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[1, 0] = "@ToDate";
                Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");

                dt = _genericUoW.GetFromSp(Parametr, "Sp_ErorrData");
                
            }
            catch (Exception ex)
            {

                return (new EmptyResult());
            }
            return PartialView(dt);
        }

        [HttpPost]
        public async Task<JsonResult> GetStation(int Projectid)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).ToList());
            else
            {
                List<long> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
                return Json(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x == n.Id) && n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            }
        }

        [HttpGet]
        public IActionResult ExportToExcel(string startDate, string endDate)
        {
            string export = "گزارش خطای سنسور " + DateTime.Now.ToPeString();
            DateTime stDate = startDate.ToGreDateTime();
            DateTime enDate = endDate.ToGreDateTime();

            string[,] Parametr = new string[2, 2];
            Parametr[0, 0] = "@FromDate";
            Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[1, 0] = "@ToDate";
            Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");


            DataTable dt = _genericUoW.GetFromSp(Parametr, "Sp_ErorrData");

            dt.Columns.Add("پروژه", typeof(string));
            dt.Columns.Add("ایستگاه", typeof(string));
            dt.Columns.Add("تاریخ", typeof(string));
            dt.Columns.Add("سنسور", typeof(string));
            dt.Columns.Add("داده", typeof(double));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["پروژه"] = (dt.Rows[i]["PrjName"]).ToString();
                dt.Rows[i]["ایستگاه"] = (dt.Rows[i]["Name"]).ToString();
                dt.Rows[i]["تاریخ"] = Convert.ToDateTime(dt.Rows[i]["DateTime"]).ToPeString("yyyy/MM/dd");
                dt.Rows[i]["سنسور"] = (dt.Rows[i]["FaName"]).ToString();
                dt.Rows[i]["داده"] = Convert.ToDouble(dt.Rows[i]["Data"]);

            }
            dt.Columns.Remove("PrjName");
            dt.Columns.Remove("Data");
            dt.Columns.Remove("DateTime");
            dt.Columns.Remove("FaName");
            dt.Columns.Remove("Name");

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