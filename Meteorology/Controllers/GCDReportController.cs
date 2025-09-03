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
    [Authorize(Roles = "AdminiStratore,GCDReport")]
    public class GCDReportController : Controller
    {

        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public GCDReportController(GenericUoW genericUoW, UserManager<User> userManager)
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
        public IActionResult GCDReport_List(int stationId, string startDate, string endDate,double min,double max)
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
                string[,] Parametr = new string[5, 2];
                Parametr[0, 0] = "@FromDate";
                Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[1, 0] = "@ToDate";
                Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[2, 0] = "@StationId";
                Parametr[2, 1] = stationId.ToString();
                Parametr[3, 0] = "@Min";
                Parametr[3, 1] = min.ToString();
                Parametr[4, 0] = "@Max";
                Parametr[4, 1] = max.ToString();
                dt = _genericUoW.GetFromSp(Parametr, "Sp_GCDReport");
                double SumData = 0;

                dt.Columns.Add("تاریخ", typeof(string));
                dt.Columns.Add("مقدار کسب شده(برحسب ساعت)", typeof(double));
                dt.Columns.Add("مجموع ساعات کسب شده", typeof(double));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["تاریخ"] = Convert.ToDateTime(dt.Rows[i]["DateTime"]).ToPeString("yyyy/MM/dd");
                    dt.Rows[i]["مقدار کسب شده(برحسب ساعت)"] = Convert.ToDouble(dt.Rows[i]["Data"]);
                    SumData += Convert.ToDouble(string.Format("{0:0.####}", dt.Rows[i]["Data"]));
                    dt.Rows[i]["مجموع ساعات کسب شده"] = SumData;

                }
                dt.Columns.Remove("Data");
                dt.Columns.Remove("DateTime");
               
            }
            catch(Exception ex)
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
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            else
            {
                List<long> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
                return Json(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x == n.Id) && n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            }
        }

        [HttpGet]
        public IActionResult ExportToExcel(int stationId,string stationname, string startDate, string endDate, double min, double max)
        {
            string export = stationname+"_"+DateTime.Now.ToPeString();
            DateTime stDate = startDate.ToGreDateTime();
            DateTime enDate = endDate.ToGreDateTime();

            string[,] Parametr = new string[5, 2];
            Parametr[0, 0] = "@FromDate";
            Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[1, 0] = "@ToDate";
            Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[2, 0] = "@StationId";
            Parametr[2, 1] = stationId.ToString();
            Parametr[3, 0] = "@Min";
            Parametr[3, 1] = min.ToString();
            Parametr[4, 0] = "@Max";
            Parametr[4, 1] = max.ToString();
            
            DataTable dt = _genericUoW.GetFromSp(Parametr, "Sp_GCDReport");
            
            double SumData = 0;

            dt.Columns.Add("تاریخ", typeof(string));
            dt.Columns.Add("مقدار کسب شده(برحسب ساعت)", typeof(double));
            dt.Columns.Add("مجموع ساعات کسب شده", typeof(double));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["تاریخ"] = Convert.ToDateTime(dt.Rows[i]["DateTime"]).ToPeString("yyyy/MM/dd");
                dt.Rows[i]["مقدار کسب شده(برحسب ساعت)"] = Convert.ToDouble(dt.Rows[i]["Data"]);
                SumData += Convert.ToDouble(string.Format("{0:0.####}", dt.Rows[i]["Data"]));
                dt.Rows[i]["مجموع ساعات کسب شده"] = SumData;

            }
            dt.Columns.Remove("Data");
            dt.Columns.Remove("DateTime");


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