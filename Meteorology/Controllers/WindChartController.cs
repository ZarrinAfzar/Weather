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

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,WindChart")]
    public class WindChartController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public WindChartController(GenericUoW genericUoW, UserManager<User> userManager)
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
        [HttpPost]
        public JsonResult GetWindSpeedSensor(int Stationid)
        {
            return Json(_genericUoW.Repository<SensorSetting>().GetAll(n => n.StationId == Stationid, n => n.SensorTypes).Where(s=>s.SensorTypes.WindSpeed).Select(n => new { n.Id, n.SensorTypes.FaName }).Distinct().ToList());
        }
        public JsonResult GetWindDirectSensor(int Stationid)
        {
            return Json(_genericUoW.Repository<SensorSetting>().GetAll(n => n.StationId == Stationid, n => n.SensorTypes).Where(s => s.SensorTypes.WindDirect).Select(n => new { n.Id, n.SensorTypes.FaName }).Distinct().ToList());
        }

        public IActionResult WindChart_filter(int Stationid, int SpeedSensor, int DirectionSensor, string FromDate, string ToDate )
        {
            DateTime fromdatetime = FromDate.ToGreDateTime();
            DateTime toDatetime = ToDate.ToGreDateTime();

            string[,] Parametr = new string[5, 2];
            Parametr[0, 0] = "@FromDate";
            Parametr[0, 1] = fromdatetime.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[1, 0] = "@ToDate";
            Parametr[1, 1] = toDatetime.ToString("yyyy-MM-dd HH:mm:ss");
            Parametr[2, 0] = "@StationId";
            Parametr[2, 1] = Stationid.ToString();
            Parametr[3, 0] = "@SpeedSensorId";
            Parametr[3, 1] = SpeedSensor.ToString();
            Parametr[4, 0] = "@DirectionSensorId";
            Parametr[4, 1] = DirectionSensor.ToString();
            DataTable Dt = _genericUoW.GetFromSp(Parametr, "Sp_GolbadChart");
             
            WindSpeedChartViewModel result = new WindSpeedChartViewModel();
            if (Dt.Rows.Count > 0)
            {

            int DirectionClass = 16;
            List<WindChartViewModel> model = new List<WindChartViewModel>();
            decimal DirectionRate = Decimal.Divide(360 , DirectionClass);
            decimal FromDirect = Decimal.Divide(DirectionRate,2);

            WindChartViewModel firstItem = new WindChartViewModel();
            int count = Dt.Rows.Count;

            firstItem.Direction = "N";
            firstItem.Speeds = GetDirectSpeeds(Dt.Select($"(Direction > {360 - FromDirect} AND Direction < 360) AND (Direction  > 0 AND  Direction < {FromDirect})").Select(n => n["Speed"]).ToList(), count);


            #region WindModel
            model.Add(firstItem);
            List<string> DirectName = new List<string>() { "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" ,"" };
            for (int i = 0; i < DirectionClass; i++)
            {
                WindChartViewModel item = new WindChartViewModel()
                {
                    Direction = DirectName[i],
                    Speeds = GetDirectSpeeds(Dt.Select($"Direction > {FromDirect} AND Direction < {DirectionRate + FromDirect}").Select(n => n["Speed"]).ToList(), count)
                };
                model.Add(item);
                FromDirect += DirectionRate;
            }
            #endregion



            #region SpeedModel
            var c1 = Math.Round((double)(Dt.Select().AsEnumerable().Count(n => Convert.ToDouble(n["Speed"]) > 0.5 && Convert.ToDouble(n["Speed"]) < 1) * 100) / count, 2);
            var c2 = Math.Round((double)(Dt.Select().AsEnumerable().Count(n => Convert.ToDouble(n["Speed"]) > 1 && Convert.ToDouble(n["Speed"]) < 2.1) * 100) / count, 2);
            var c3 = Math.Round((double)(Dt.Select().AsEnumerable().Count(n => Convert.ToDouble(n["Speed"]) > 2.1 && Convert.ToDouble(n["Speed"]) < 3.6) * 100) / count, 2);
            var c4 = Math.Round((double)(Dt.Select().AsEnumerable().Count(n => Convert.ToDouble(n["Speed"]) > 3.6 && Convert.ToDouble(n["Speed"]) < 5.6) * 100) / count, 2);
            var c5 = Math.Round((double)(Dt.Select().AsEnumerable().Count(n => Convert.ToDouble(n["Speed"]) > 5.6 && Convert.ToDouble(n["Speed"]) < 8.8) * 100) / count, 2);
            var c6 = Math.Round((double)(Dt.Select().AsEnumerable().Count(n => Convert.ToDouble(n["Speed"]) > 8.8 && Convert.ToDouble(n["Speed"]) < 11.1) * 100) / count, 2);
            var c7 = Math.Round(((double)(Dt.Select().AsEnumerable().Count(n => Convert.ToDouble(n["Speed"]) > 11 / 1) * 100) / count), 2);
            List<SpeedChartViewModel> speedmodel = new List<SpeedChartViewModel>()
            {
                new SpeedChartViewModel()
                {
                    title = "0.5 -> 1",
                    value = c1.ToString()
                },
                new SpeedChartViewModel()
                {
                    title = "1 -> 2.1",
                    value = c2.ToString()
                },
                new SpeedChartViewModel()
                {
                    title = "2.1 -> 3.6",
                    value = c3.ToString()
                },
                new SpeedChartViewModel()
                {
                    title = "3.6 -> 5.6",
                    value = c4.ToString()
                },
                new SpeedChartViewModel()
                {
                    title = "5.6 -> 8.8",
                    value = c5.ToString()
                },
                new SpeedChartViewModel()
                {
                    title = "8.8 -> 11.1",
                    value = c6.ToString()
                },
                new SpeedChartViewModel()
                {
                    title = "> 11.1 ",
                    value = c7.ToString()
                }
            };
            #endregion



            result.WindChartViewModels = model;
            result.SpeedChartViewModels = speedmodel;
            }
            else
            {
                result.WindChartViewModels = new List<WindChartViewModel>();
                result.SpeedChartViewModels = new List<SpeedChartViewModel>();
            }
            return PartialView("ColumnChartWind", result);
        }
         

        private List<object> GetDirectSpeeds(List<object> ThisDirectionAllData, int AllData) 
        {
            List<double> range = new List<double>() { 0.5, 1, 2.1, 3.6, 5.7, 8.8, 11.1 };
            List<object> model = new List<object>();
            if(ThisDirectionAllData.Count > 0)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (i == 6)
                    {
                        model.Add(Math.Round((double)(ThisDirectionAllData.Count(n => (double)n > range[i]) * 100) / AllData, 2));
                    }
                    else
                    {
                        model.Add(Math.Round((double)(ThisDirectionAllData.Count(n => (double)n > range[i] && (double)n < range[i + 1]) * 100) / AllData, 2));
                    }
                }
            }
            else
            {
                model.Add(0.0);
                model.Add(0.0);
                model.Add(0.0);
                model.Add(0.0);
                model.Add(0.0);
                model.Add(0.0);
                model.Add(0.0);
            }

            return model;
        }
    }
}