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
    [Authorize(Roles = "AdminiStratore,GapReport")]
    public class GapReportController : Controller
    {

        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public GapReportController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public IActionResult GapReport_List(string startDate, string endDate)
        {
            DataTable dt;
            try
            {
                DateTime stDate = startDate.ToGreDateTime();
                DateTime enDate = endDate.ToGreDateTime();

                string[,] Parametr = new string[2, 2];
                Parametr[0, 0] = "@FromDate";
                Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[1, 0] = "@ToDate";
                Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
                dt = _genericUoW.GetFromSp(Parametr, "Sp_GapData");
               
            }
            catch (Exception ex)
            {

                return (new EmptyResult());
            }
            return PartialView(dt);
        }


        [HttpGet]
        public IActionResult ExportToExcel(string startDate, string endDate)
        {
            DataTable dt;
            string export = "GapReport_" + startDate + endDate;
            DateTime stDate = startDate.ToGreDateTime();
            DateTime enDate = endDate.ToGreDateTime();

            string[,] Parametr = new string[2, 2];
            Parametr[0, 0] = "@FromDate";
            Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[1, 0] = "@ToDate";
            Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
            dt = _genericUoW.GetFromSp(Parametr, "Sp_GapData");
            

            dt.Columns.Add("استان", typeof(string));
            dt.Columns.Add("ایستگاه", typeof(string));
            dt.Columns.Add("شماره سیم کارت", typeof(string));
            dt.Columns.Add("حداقل فاصله زمانی", typeof(int));
            dt.Columns.Add("حداکثر فاصله زمانی", typeof(int));
            dt.Columns.Add("آخرین تاریخ ارسال داده", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["استان"] = dt.Rows[i]["PrjName"];
                dt.Rows[i]["ایستگاه"] = dt.Rows[i]["Name"];
                dt.Rows[i]["شماره سیم کارت"] = dt.Rows[i]["StationCardNumber"];
                dt.Rows[i]["حداقل فاصله زمانی"] = dt.Rows[i]["time"];
                dt.Rows[i]["حداکثر فاصله زمانی"] = dt.Rows[i]["gap"];
                dt.Rows[i]["آخرین تاریخ ارسال داده"] = Convert.ToDateTime((dt.Rows[i]["lastDate"])).ToPeString("yyyy/MM/dd HH:mm");
            }
            dt.Columns.Remove("PrjName");
            dt.Columns.Remove("Name");
            dt.Columns.Remove("StationCardNumber");
            dt.Columns.Remove("time");
            dt.Columns.Remove("gap");
            dt.Columns.Remove("lastDate");
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