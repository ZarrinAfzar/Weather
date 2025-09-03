using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Weather.Data.Base;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,Chart")]
    public class ChartController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public ChartController(GenericUoW genericUoW, UserManager<User> userManager)
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
                ProjectList = new SelectList(_genericUoW.Repository<Project>().GetAll(n => StationUserId.Any(x => x.ProjectId == n.Id)).OrderBy(m => m.Name).Select(n => new { n.Id, n.Name }).ToList(), "Id", "Name");
            }
            ViewBag.ProjectList = ProjectList;

            return View();
        }

        public JsonResult ChartValue(int stationId, string startDate, string endDate, string DateType, string computingType_Right, string computingType_Left, int Sensor_Right, int Sensor_Left)
        {

            try
            {

                DateTime fromdatetime = startDate.ToGreDateTime();
                DateTime toDatetime = !string.IsNullOrEmpty(endDate) ? endDate.ToGreDateTime() : DateTime.Now;

                List<double> rightdata = new List<double>();
                List<string> RangeDate = new List<string>();
                double min = 0;
                double max = 0;
                if (Sensor_Right != 0)
                {
                    string[,] Parametr = new string[6, 2];
                    Parametr[0, 0] = "@FromDate";
                    Parametr[0, 1] = fromdatetime.ToString("yyyy-MM-dd HH:mm:ss");
                    Parametr[1, 0] = "@ToDate";
                    Parametr[1, 1] = toDatetime.ToString("yyyy-MM-dd HH:mm:ss");
                    Parametr[2, 0] = "@DateType";
                    Parametr[2, 1] = DateType;
                    Parametr[3, 0] = "@ComputingType";
                    Parametr[3, 1] = string.IsNullOrEmpty(computingType_Right) ? " " : computingType_Right;
                    Parametr[4, 0] = "@StationId";
                    Parametr[4, 1] = stationId.ToString();
                    Parametr[5, 0] = "@SensorId";
                    Parametr[5, 1] = getIds(stationId, Sensor_Right);
                    DataTable dt_rightdata = _genericUoW.GetFromSp(Parametr, "Sp_SensorChart");

                    foreach (DataRow row in dt_rightdata.Rows)
                    {
                        min = min > (double)row["Data"] ? (double)row["Data"] : min;
                        max = max < (double)row["Data"] ? (double)row["Data"] : max;
                        rightdata.Add((double)row["Data"]);
                        RangeDate.Add(Convert.ToDateTime(row["DateTime"]).ToPeString("yyyy/MM/dd HH:mm"));
                    }
                    //RangeDate = dt_rightdata.Select().AsEnumerable().Select(n => Convert.ToDateTime(n["DateTime"]).ToPeString("yyyy/MM/dd HH:mm:ss")).ToList();
                }
                List<double> leftdata = new List<double>();
                if (Sensor_Left != 0)
                {
                    string[,] Parametr = new string[6, 2];
                    Parametr[0, 0] = "@FromDate";
                    Parametr[0, 1] = fromdatetime.ToString("yyyy-MM-dd HH:mm:ss");
                    Parametr[1, 0] = "@ToDate";
                    Parametr[1, 1] = toDatetime.ToString("yyyy-MM-dd HH:mm:ss");
                    Parametr[2, 0] = "@DateType";
                    Parametr[2, 1] = DateType;
                    Parametr[3, 0] = "@ComputingType";
                    Parametr[3, 1] = string.IsNullOrEmpty(computingType_Left) ? " " : computingType_Left;
                    Parametr[4, 0] = "@StationId";
                    Parametr[4, 1] = stationId.ToString();
                    Parametr[5, 0] = "@SensorId";
                    Parametr[5, 1] = getIds(stationId, Sensor_Left);
                    DataTable dt_leftdata = _genericUoW.GetFromSp(Parametr, "Sp_SensorChart");
                    //leftdata = dt_leftdata.Select().AsEnumerable().Select(n => Convert.ToDouble(n["Data"].ToString().Trim(), CultureInfo.InvariantCulture)).ToList(); 
                    foreach (DataRow row in dt_leftdata.Rows)
                    {
                        leftdata.Add((double)row["Data"]);
                        min = min > (double)row["Data"] ? (double)row["Data"] : min;
                        max = max < (double)row["Data"] ? (double)row["Data"] : max;
                    }
                    if (RangeDate.Count == 0)
                        foreach (DataRow row in dt_leftdata.Rows)
                            RangeDate.Add(Convert.ToDateTime(row["DateTime"]).ToPeString("yyyy/MM/dd HH:mm"));
                    //RangeDate = dt_leftdata.Select().AsEnumerable().Select(n => Convert.ToDateTime(n["DateTime"]).ToPeString("yyyy/MM/dd HH:mm:ss")).ToList();
                }

                //RangeDate = reportType_Right == "AllData" ? dt_rightdata.Select().AsEnumerable().Select(n => Convert.ToDateTime(n["DateTime"]).ToPeString("HH:mm:ss yyyy/MM/dd")).ToList() :
                //                                dt_rightdata.Select().AsEnumerable().Select(n => Convert.ToDateTime(n["DateTime"]).ToPeString("yyyy/MM/dd")).ToList();
                //RangeDate = reportType_Right == "AllData" ? dt_leftdata.Select().AsEnumerable().Select(n => Convert.ToDateTime(n["DateTime"]).ToPeString("HH:mm:ss yyyy/MM/dd")).ToList() :
                //            dt_leftdata.Select().AsEnumerable().Select(n => Convert.ToDateTime(n["DateTime"]).ToPeString("yyyy/MM/dd")).ToList();

                return Json(new { date = RangeDate, data1 = rightdata, data2 = leftdata, min, max, step = 1 });
            }
            catch (Exception ex)
            {
                return Json(null);
            }
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
        public JsonResult GetSensor(int Stationid)
        {
            return Json(_genericUoW.Repository<SensorSetting>().GetAll(n => n.StationId == Stationid, n => n.SensorTypes).Select(n => new { n.SensorTypes.Id, n.SensorTypes.FaName }).OrderBy(m => m.FaName).Distinct().ToList());
        }


        private string getIds(int stationId, int sensortypeid)
        {
            try
            {

                var sensorId = _genericUoW.Repository<SensorSetting>().GetAll(n => n.StationId == stationId, n => n.SensorTypes).Where(n => n.SensorTypes.Id == sensortypeid).Select(n => new { n.Id }).ToList();
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