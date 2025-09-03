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
    [Authorize(Roles = "AdminiStratore,GDDReport")]
    public class GDDReportController : Controller
    {

        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public GDDReportController(GenericUoW genericUoW, UserManager<User> userManager)
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
        public IActionResult GDDReport_List(int stationId, string startDate, string endDate, double Tmin, double Tmax, string DDType)
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
                string[,] Parametr = new string[6, 2];
                Parametr[0, 0] = "@FromDate";
                Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[1, 0] = "@ToDate";
                Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[2, 0] = "@SensorSettingId";
                Parametr[2, 1] = getIds(stationId, 5);
                Parametr[3, 0] = "@Tmin";
                Parametr[3, 1] = Tmin.ToString();
                Parametr[4, 0] = "@Tmax";
                Parametr[4, 1] = Tmax.ToString();
                Parametr[5, 0] = "@DDType";
                Parametr[5, 1] = DDType;
                dt = _genericUoW.GetFromSp(Parametr, "Sp_CalculatePDD");
                double SumData = 0;

                dt.Columns.Add("مقدار روزانه درجه روز", typeof(double));
                dt.Columns.Add("مجموع درجه روز رشد", typeof(double));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["مقدار روزانه درجه روز"] = Convert.ToDouble(string.Format("{0:0.####}", dt.Rows[i]["PDD"]));
                    SumData += Convert.ToDouble(string.Format("{0:0.####}", dt.Rows[i]["PDD"]));
                    dt.Rows[i]["مجموع درجه روز رشد"] = SumData;

                }
                dt.Columns.Remove("PDD");

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
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            else
            {
                List<long> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
                return Json(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x == n.Id) && n.ProjectId == Projectid).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList());
            }
        }

        [HttpGet]
        public IActionResult ExportToExcel(int stationId, string stationname, string startDate, string endDate, double Tmin, double Tmax, string DDType)
        {
            string export = stationname + "_" + DateTime.Now.ToPeString();
            DateTime stDate = startDate.ToGreDateTime();
            DateTime enDate = endDate.ToGreDateTime();

            string[,] Parametr = new string[6, 2];
            Parametr[0, 0] = "@FromDate";
            Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[1, 0] = "@ToDate";
            Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[2, 0] = "@SensorSettingId";
            Parametr[2, 1] = getIds(stationId, 5);
            Parametr[3, 0] = "@Tmin";
            Parametr[3, 1] = Tmin.ToString();
            Parametr[4, 0] = "@Tmax";
            Parametr[4, 1] = Tmax.ToString();
            Parametr[5, 0] = "@DDType";
            Parametr[5, 1] = DDType;
            DataTable dt = _genericUoW.GetFromSp(Parametr, "Sp_CalculatePDD");

            double SumData = 0;

            dt.Columns.Add("تاریخ", typeof(string));
            dt.Columns.Add("مقدار روزانه درجه روز", typeof(double));
            dt.Columns.Add("مجموع درجه روز رشد", typeof(double));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["تاریخ"] = Convert.ToDateTime(dt.Rows[i]["DateTime"]).ToPeString("yyyy/MM/dd");
                dt.Rows[i]["مقدار روزانه درجه روز"] = Convert.ToDouble(string.Format("{0:0.####}",dt.Rows[i]["PDD"]));
                SumData += Convert.ToDouble(string.Format("{0:0.####}", dt.Rows[i]["PDD"]));
                dt.Rows[i]["مجموع درجه روز رشد"] = SumData;

            }
            dt.Columns.Remove("PDD");
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

        private string getIds(double stationId, long sensortypeid)
        {
            try
            {

                var sensorId = _genericUoW.Repository<SensorSetting>().GetAll(n => n.StationId == stationId && n.SensorTypeId == sensortypeid).Select(n => new { n.Id }).ToList();
                string Ids = "" + sensorId[0].Id.ToString();
                for (int i = 1; i < sensorId.Count; i++)
                {
                    Ids += "," + sensorId[i].Id.ToString();
                }

                return Ids;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}