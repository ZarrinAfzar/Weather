using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weather.Data.Base;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,DDChart")]
    public class DDChartController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public DDChartController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
        }
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            SelectList ProjectList = null;
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().OrderBy(m => m.Name).Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            }
            else
            {
                List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
                ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.Id)).OrderBy(m => m.Name).Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
            }
            ViewBag.ProjectList = ProjectList;
            return View();
        }

        public JsonResult DDChartValue(string DDType, int AlarmId)
        {
            Alarm DDAlarm = _genericUoW.Repository<Alarm>().GetById(AlarmId);
            PestAlarmDetail alarmDetail = _genericUoW.Repository<PestAlarmDetail>().GetAll().Where(a => a.AlarmId == AlarmId).FirstOrDefault();
            List<double> data = new List<double>();
            List<string> RangeDate = new List<string>();
            string[,] Parametr = new string[6, 2];

            Parametr[0, 0] = "@FromDate";
            Parametr[0, 1] = DDAlarm.AlarmStartDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[1, 0] = "@ToDate";
            Parametr[1, 1] = DDAlarm.AlarmEndDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[2, 0] = "@SensorSettingId";
            Parametr[2, 1] = getIds(DDAlarm.StationId, 5);
            Parametr[3, 0] = "@Tmin";
            Parametr[3, 1] = DDAlarm.PestAlarmDetail.TempBase.ToString();
            Parametr[4, 0] = "@Tmax";
            Parametr[4, 1] = DDAlarm.PestAlarmDetail.TempMax.ToString();
            Parametr[5, 0] = "@DDType";
            Parametr[5, 1] = DDType;
            DataTable dt_data = _genericUoW.GetFromSp(Parametr, "Sp_CalculatePDD");
            double SumData = 0;
            foreach (DataRow row in dt_data.Rows)
            {
                SumData += Convert.ToDouble(string.Format("{0:0.####}", row["PDD"]));
                
                data.Add(SumData);
                RangeDate.Add(Convert.ToDateTime(row["DateTime"]).ToPeString("yyyy/MM/dd"));
            }
            //RangeDate = dt_rightdata.Select().AsEnumerable().Select(n => Convert.ToDateTime(n["DateTime"]).ToPeString("yyyy/MM/dd HH:mm:ss")).ToList();
            return Json(new { date = RangeDate, data = data, max = SumData + 1 });
        }
        [HttpGet]
        public IActionResult ExportToExcel(string stationname,string DDType, int AlarmId)
        {
            string export = stationname + "_" + DateTime.Now.ToPeString();

            Alarm DDAlarm = _genericUoW.Repository<Alarm>().GetById(AlarmId);
            PestAlarmDetail alarmDetail = _genericUoW.Repository<PestAlarmDetail>().GetAll().Where(a => a.AlarmId == AlarmId).FirstOrDefault();
            List<double> data = new List<double>();
            List<string> RangeDate = new List<string>();
           
            string[,] Parametr = new string[6, 2];

            Parametr[0, 0] = "@FromDate";
            Parametr[0, 1] = DDAlarm.AlarmStartDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[1, 0] = "@ToDate";
            Parametr[1, 1] = DDAlarm.AlarmEndDate.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[2, 0] = "@SensorSettingId";
            Parametr[2, 1] = getIds(DDAlarm.StationId, 5);
            Parametr[3, 0] = "@Tmin";
            Parametr[3, 1] = DDAlarm.PestAlarmDetail.TempBase.ToString();
            Parametr[4, 0] = "@Tmax";
            Parametr[4, 1] = DDAlarm.PestAlarmDetail.TempMax.ToString();
            Parametr[5, 0] = "@DDType";
            Parametr[5, 1] = DDType;
            DataTable dt = _genericUoW.GetFromSp(Parametr, "Sp_CalculatePDD");

            double SumData = 0;
          
            dt.Columns.Add("تاریخ", typeof(string));
            dt.Columns.Add("درجه روز رشد", typeof(double));
            dt.Columns.Add("مجموع درجه روز رشد", typeof(double));
           
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["تاریخ"] = Convert.ToDateTime(dt.Rows[i]["DateTime"]).ToPeString("yyyy/MM/dd");
                dt.Rows[i]["درجه روز رشد"] = Convert.ToDouble(dt.Rows[i]["PDD"]);
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

        [HttpPost]
        public async Task<JsonResult> GetStation(int Projectid)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                return Json(_genericUoW.Repository<Station>().GetAll(n => n.ProjectId == Projectid).OrderBy(m => m.Name).Select(n => new { n.Id, n.Name }).ToList());
            else
            {
                List<long> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
                return Json(_genericUoW.Repository<Station>().GetAll(n => StationUserId.Any(x => x == n.Id) && n.ProjectId == Projectid).OrderBy(m => m.Name).Select(n => new { n.Id, n.Name }).ToList());
            }
        }
        [HttpPost]
        public JsonResult GetAlarms(int Stationid)
        {
            return Json(_genericUoW.Repository<Alarm>().GetAll(n => n.StationId == Stationid && n.AlarmType == EnuAlarm.Pest).Select(n => new { n.Id, n.AlarmName }).OrderBy(m => m.AlarmName).ToList());


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

            //MAX(CASE WHEN[WeatherDB].[dbo].[SensorSetting].Id = 12 THEN[Data] ELSE NULL END) as max
        }
    }
}