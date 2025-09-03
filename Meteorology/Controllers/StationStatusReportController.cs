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
    [Authorize(Roles = "AdminiStratore,StationStatus")]
    public class StationStatusReportController : Controller
    {

        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public StationStatusReportController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult StationStatusReport_List(string StartDate)
        {
            DataTable dt;
            try
            {
                DateTime stDate;

                if (StartDate == null)
                {
                    stDate = DateTime.Now.AddHours(-1);

                    string[,] Parametr = new string[1, 2];
                    Parametr[0, 0] = "@FromDate";
                    Parametr[0, 1] =  stDate.ToString("yyyy-MM-dd HH:mm:ss");
                    dt = _genericUoW.GetFromSp(Parametr, "Sp_StationStatus");
                }
                else
                {
                    var st = DateTime.Now.ToPeString();
                    var sss = st.ToGreDateTime();
                    stDate = StartDate.ToGreDateTime();
                    string[,] Parametr = new string[1, 2];
                    Parametr[0, 0] = "@FromDate";
                    Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");

                    dt = _genericUoW.GetFromSp(Parametr, "Sp_StationStatus1");
                }
            }
            catch (Exception ex)
            {

                return (new EmptyResult());
            }
            return PartialView(dt);
        }


        [HttpGet]
        public IActionResult ExportToExcel(string StartDate)
        {
            DataTable dt;
            string export = "StationStatus_" + StartDate?? DateTime.Now.ToPeString();
           
                DateTime stDate;

                if (StartDate == null)
                {
                    stDate = DateTime.Now.AddHours(-1);

                    string[,] Parametr = new string[1, 2];
                    Parametr[0, 0] = "@FromDate";
                    Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");

                    dt = _genericUoW.GetFromSp(Parametr, "Sp_StationStatus");
                }
                else
                {
                    stDate = StartDate.ToGreDateTime();
                    string[,] Parametr = new string[1, 2];
                    Parametr[0, 0] = "@FromDate";
                    Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");

                    dt = _genericUoW.GetFromSp(Parametr, "Sp_StationStatus1");

                }
                 
           

            dt.Columns.Add("پروژه", typeof(string));
            dt.Columns.Add("ایستگاه", typeof(string));
            dt.Columns.Add("شماره سیم کارت", typeof(string));
            dt.Columns.Add("آخرین تاریخ ارسال داده", typeof(string));
            dt.Columns.Add("باران 24 ساعت", typeof(double));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["پروژه"] = dt.Rows[i]["PrjName"];
                dt.Rows[i]["ایستگاه"] = dt.Rows[i]["Name"];
                dt.Rows[i]["شماره سیم کارت"] = dt.Rows[i]["StationCardNumber"];
                dt.Rows[i]["آخرین تاریخ ارسال داده"] = Convert.ToDateTime((dt.Rows[i]["lastDate"])).ToPeString("yyyy/MM/dd HH:mm");
                dt.Rows[i]["باران 24 ساعت"] = dt.Rows[i]["SumRain"];
            }
            dt.Columns.Remove("PrjName");
            dt.Columns.Remove("Name");
            dt.Columns.Remove("StationCardNumber");
            dt.Columns.Remove("lastDate");
            dt.Columns.Remove("SumRain");
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