using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Base;
using Weather.Data.UnitOfWork;
using Weather.Models;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,SensorReport")]
    public class SensorReportController : Controller
    {

        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public SensorReportController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.pagecount = 6;
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                //ViewBag.ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).OrderBy(m => m.Name).ToList(), "Id", "Name");
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
        public IActionResult SensorReport_List(int stationId, string startDate, string endDate, string reportType, string computingType, List<long> sensorselected, int Skip = 0)
        {
            try
            {
                DateTime stDate = startDate.ToGreDateTime();
                DateTime enDate = endDate.ToGreDateTime();

                string[,] Parametr = new string[7, 2];
                Parametr[0, 0] = "@FromDate"; Parametr[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[1, 0] = "@ToDate"; Parametr[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parametr[2, 0] = "@StationId"; Parametr[2, 1] = stationId.ToString();
                Parametr[3, 0] = "@DateType"; Parametr[3, 1] = reportType;
                Parametr[4, 0] = "@Fields"; Parametr[4, 1] = string.IsNullOrEmpty(computingType) ? "" : getField(stationId, computingType);
                Parametr[5, 0] = "@Take"; Parametr[5, 1] = int.MaxValue.ToString();
                Parametr[6, 0] = "@Skip"; Parametr[6, 1] = "0";

                string spName = reportType.Equals("DAY", StringComparison.OrdinalIgnoreCase)
                    ? "Sp_SensorData_Shifted"
                    : "Sp_SensorData";

                DataTable model = _genericUoW.GetFromSp(Parametr, spName);

                var allSensorFaNames = _genericUoW.Repository<SensorSetting>()
                    .GetAll(n => n.StationId == stationId, n => n.SensorTypes)
                    .Select(n => n.SensorTypes.FaName)
                    .Distinct()
                    .ToList();

                List<string> selectedFaNames = (sensorselected != null && sensorselected.Any())
                    ? _genericUoW.Repository<SensorSetting>()
                        .GetAll(n => n.StationId == stationId && sensorselected.Contains(n.SensorTypes.Id), n => n.SensorTypes)
                        .Select(n => n.SensorTypes.FaName)
                        .Distinct()
                        .ToList()
                    : allSensorFaNames;

                Func<string, string> norm = s =>
                {
                    if (string.IsNullOrWhiteSpace(s)) return "";
                    var t = s.Trim();
                    t = t.Replace("\u200c", " ");
                    t = t.Replace("ي", "ی").Replace("ك", "ک");
                    while (t.Contains("  ")) t = t.Replace("  ", " ");
                    return t;
                };

                Func<string, string> baseName = name =>
                {
                    if (string.Equals(name, "DateTime", StringComparison.OrdinalIgnoreCase)) return "DateTime";
                    var stripped = System.Text.RegularExpressions.Regex.Replace(name ?? "", @"\s*\(.*?\)\s*", "");
                    return norm(stripped);
                };

                var allowedBase = new HashSet<string>(
                    selectedFaNames.Select(baseName).Append("DateTime"),
                    StringComparer.OrdinalIgnoreCase
                );

                var toRemove = model.Columns.Cast<DataColumn>()
                    .Where(c => !allowedBase.Contains(baseName(c.ColumnName)))
                    .Select(c => c.ColumnName)
                    .ToList();

                foreach (var col in toRemove)
                    model.Columns.Remove(col);

                ViewBag.DateType = reportType;

                if (model != null && model.Rows.Count > 0)
                    return PartialView(model);

                ViewBag.MyHtml = "<h4 style=\"text-align:center;color:red\">اطلاعاتی برای نمایش وجود ندارد</h4>";
                return PartialView(model);
            }
            catch (Exception)
            {
                return new EmptyResult();
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

        [HttpPost]
        public async Task<JsonResult> SelecteSensors(int StationId, string computingType)
        {
            var repo = _genericUoW.Repository<SensorSetting>();

            var types = string.IsNullOrEmpty(computingType)
                ? new List<string>()
                : computingType.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => x.Trim().ToUpper())
                               .ToList();

            IQueryable<SensorSetting> query = repo.GetAll(n => n.StationId == StationId, n => n.SensorTypes).AsQueryable();

            if (!types.Any())
            {
                var all = query.Select(n => new { n.SensorTypes.Id, n.SensorTypes.FaName })
                               .OrderBy(m => m.FaName)
                               .Distinct()
                               .ToList();
                return Json(all);
            }

            query = query.Where(n =>
                (types.Contains("AVG") && n.SensorTypes.AVG) ||
                (types.Contains("MIN") && n.SensorTypes.Min) ||
                (types.Contains("MAX") && n.SensorTypes.Max) ||
                (types.Contains("SUM") && n.SensorTypes.Sum));

            var result = query.Select(n => new { n.SensorTypes.Id, n.SensorTypes.FaName })
                              .OrderBy(m => m.FaName)
                              .Distinct()
                              .ToList();

            return Json(result);
        }

        private string getField(int stationId, string computingType)
        {
            // گرفتن همه سنسورهای فعال با navigation properties لازم
            var sensorTypes = _genericUoW.Repository<SensorSetting>()
                .GetAll(
                    n => n.StationId == stationId && n.SensorEnable,
                    n => n.SensorTypes,
                    n => n.SensorTypes.Unit
                )
                .Select(n => new
                {
                    SensorId = n.Id,
                    FaName = n.SensorTypes?.FaName ?? "",
                    MinEnabled = n.SensorTypes?.Min ?? false,
                    MaxEnabled = n.SensorTypes?.Max ?? false,
                    AvgEnabled = n.SensorTypes?.AVG ?? false,
                    SumEnabled = n.SensorTypes?.Sum ?? false,
                    UnitName = n.SensorTypes?.Unit?.EnName ?? ""
                })
                .OrderBy(n => n.FaName)
                .ToList();

            string result = "";

            var grouped = sensorTypes.GroupBy(s => s.FaName);

            foreach (var group in grouped)
            {
                var sensorIds = group.Select(s => s.SensorId).ToList();
                string idsStr = string.Join(",", sensorIds);
                string faName = group.Key;
                string unitName = group.Select(s => s.UnitName).FirstOrDefault() ?? "";

                // Max
                if (computingType.Contains("Max") && group.Any(s => s.MaxEnabled))
                {
                    result += $",Round(MAX(CASE WHEN SensorSetting.Id in({idsStr}) THEN Data ELSE NULL END),2) as N'{faName} ({unitName})'";
                }
                // Min
                if (computingType.Contains("Min") && group.Any(s => s.MinEnabled))
                {
                    result += $",Round(MIN(CASE WHEN SensorSetting.Id in({idsStr}) THEN Data ELSE NULL END),2) as N'{faName} ({unitName})'";
                }
                // Avg
                if (computingType.Contains("Avg") && group.Any(s => s.AvgEnabled))
                {
                    result += $",Round(AVG(CASE WHEN SensorSetting.Id in({idsStr}) THEN Data ELSE NULL END),2) as N'{faName} ({unitName})'";
                }
                // Sum
                if (computingType.Contains("Sum") && group.Any(s => s.SumEnabled))
                {
                    result += $",Round(SUM(CASE WHEN SensorSetting.Id in({idsStr}) THEN Data ELSE NULL END),2) as N'{faName} ({unitName})'";
                }
            }

            return result;
        }

        [HttpGet]
        public IActionResult ExportToExcel(int stationId, string stationname, string startDate, string endDate, string reportType, string computingType, string sensorselected)
        {
            try
            {
                string export = stationname + "_" + DateTime.Now.ToPeString("yyyyMMdd_HHmm");
                DateTime stDate = startDate.ToGreDateTime();
                DateTime enDate = endDate.ToGreDateTime();

                // Parse selected sensor ids
                var ids = new List<long>();
                if (!string.IsNullOrWhiteSpace(sensorselected))
                {
                    ids = sensorselected
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => long.Parse(s.Trim()))
                            .ToList();
                }

                //  Build SP params (match list view)
                DataTable model;
                var fields = string.IsNullOrEmpty(computingType) ? "" : getField(stationId, computingType);

                if (reportType.Equals("DAY", StringComparison.OrdinalIgnoreCase))
                {
                    // call shifted to match UI behavior
                    string[,] p = new string[7, 2];
                    p[0, 0] = "@FromDate"; p[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
                    p[1, 0] = "@ToDate"; p[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
                    p[2, 0] = "@StationId"; p[2, 1] = stationId.ToString();
                    p[3, 0] = "@DateType"; p[3, 1] = reportType;
                    p[4, 0] = "@Fields"; p[4, 1] = fields;
                    p[5, 0] = "@Take"; p[5, 1] = int.MaxValue.ToString();
                    p[6, 0] = "@Skip"; p[6, 1] = "0";

                    model = _genericUoW.GetFromSp(p, "Sp_SensorData_Shifted");
                }
                else
                {
                    string[,] p = new string[5, 2];
                    p[0, 0] = "@FromDate"; p[0, 1] = stDate.ToString("yyyy-MM-dd HH:mm:ss");
                    p[1, 0] = "@ToDate"; p[1, 1] = enDate.ToString("yyyy-MM-dd HH:mm:ss");
                    p[2, 0] = "@StationId"; p[2, 1] = stationId.ToString();
                    p[3, 0] = "@DateType"; p[3, 1] = reportType;
                    p[4, 0] = "@Fields"; p[4, 1] = fields;

                    model = _genericUoW.GetFromSp(p, "Sp_SensorDataExport");
                }

                //  Collect allowed FA names (only selected sensors)
                var allSensorFaNames = _genericUoW.Repository<SensorSetting>()
                    .GetAll(n => n.StationId == stationId, n => n.SensorTypes)
                    .Select(n => n.SensorTypes.FaName)
                    .Distinct()
                    .ToList();

                var selectedFaNames = (ids != null && ids.Count > 0)
                    ? _genericUoW.Repository<SensorSetting>()
                        .GetAll(n => n.StationId == stationId && ids.Contains(n.SensorTypes.Id), n => n.SensorTypes)
                        .Select(n => n.SensorTypes.FaName)
                        .Distinct()
                        .ToList()
                    : allSensorFaNames;

                //  Same normalization helpers as list action
                Func<string, string> norm = s =>
                {
                    if (string.IsNullOrWhiteSpace(s)) return "";
                    var t = s.Trim();
                    t = t.Replace("\u200c", " ");
                    t = t.Replace("ي", "ی").Replace("ك", "ک");
                    while (t.Contains("  ")) t = t.Replace("  ", " ");
                    return t;
                };
                Func<string, string> baseName = name =>
                {
                    if (string.Equals(name, "DateTime", StringComparison.OrdinalIgnoreCase)) return "DateTime";
                    var stripped = System.Text.RegularExpressions.Regex.Replace(name ?? "", @"\s*\(.*?\)\s*", "");
                    return norm(stripped);
                };

                var allowedBase = new HashSet<string>(
                    selectedFaNames.Select(baseName).Append("DateTime"),
                    StringComparer.OrdinalIgnoreCase
                );

                //  Drop extra columns so Excel == UI
                var toRemove = model.Columns.Cast<DataColumn>()
                    .Where(c => !allowedBase.Contains(baseName(c.ColumnName)))
                    .Select(c => c.ColumnName)
                    .ToList();

                foreach (var col in toRemove)
                    model.Columns.Remove(col);

                //  Move DateTime to first & format Persian
                if (model.Columns.Contains("DateTime"))
                {
                    model.Columns.Add("تاریخ", typeof(string));
                    if (reportType.Equals("DAY", StringComparison.OrdinalIgnoreCase) || reportType.Equals("MONTH", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (DataRow row in model.Rows)
                            row["تاریخ"] = Convert.ToDateTime(row["DateTime"]).ToPeString("18:30:00 yyyy/MM/dd");
                    }
                    else
                    {
                        foreach (DataRow row in model.Rows)
                            row["تاریخ"] = Convert.ToDateTime(row["DateTime"]).ToPeString("HH:mm:ss yyyy/MM/dd");
                    }


                    model.Columns.Remove("DateTime");
                    model.Columns["تاریخ"].SetOrdinal(0);
                }

                // 7) Build Excel (EPPlus)
                byte[] fileContents;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(export);
                    worksheet.Cells["A1"].LoadFromDataTable(model, true);
                    using (var range = worksheet.Cells[1, 1, 1, model.Columns.Count])
                        range.Style.Font.Bold = true;

                    worksheet.Cells.AutoFitColumns();
                    fileContents = package.GetAsByteArray();
                }

                if (fileContents == null || fileContents.Length == 0)
                    return NotFound();

                return File(
                    fileContents,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    export + ".xlsx"
                );
            }
            catch (Exception ex)
            {
                return Content("خطا در تولید فایل اکسل: " + ex.Message);
            }
        }

    }
}