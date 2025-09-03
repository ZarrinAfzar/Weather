using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data;
using Weather.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using Weather.Data.ViewModel;
using Weather.Data.Base;

namespace Weather.Controllers
{
    [Authorize]
    public class MapController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;

        public MapController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _genericUoW = genericUoW;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            List<Station> stationList = new List<Station>();
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
                stationList = _genericUoW.Repository<Station>().GetAllByQuery(null, n => n.StationType).ToList();
            else
            {
                List<long> StationId = _genericUoW.Repository<UserStation>().GetAllByQuery(n => n.UserId == user.Id).Select(n => n.StationId).ToList();
                stationList = _genericUoW.Repository<Station>().GetAllByQuery(n => StationId.Any(x => x == n.Id), n => n.StationType).ToList();
            }



            List<MapViewModel> model = new List<MapViewModel>();
            string[,] Parametr = new string[1, 2];
            Parametr[0, 0] = "@myList";
            Parametr[0, 1] = stationList.Count > 0 ? "(" + string.Join(",", stationList.Select(n => n.Id).ToList()) + ")" : "(0)";
            List<sensorLastData> AllsensorLastDatas = DataTableToModel.ConvertDataTable<sensorLastData>(_genericUoW.GetFromSp(Parametr, "Sp_MapInfo"));
            foreach (var item in stationList)
            {
                MapViewModel m = new MapViewModel();
                m.StationName = item.Name;
                m.Latitude = item.Latitude;
                m.Longitude = item.Longitude;
                m.sensorLastDatas = AllsensorLastDatas.Where(n => n.StationId == item.Id).ToList();
                m.DateTime = m.sensorLastDatas.Count > 0 ? m.sensorLastDatas[0].DateTime.ToPeString("yyyy/MM/dd HH:mm:ss") : DateTime.Now.ToPeString("yyyy/MM/dd HH:mm:ss");
                TimeSpan Difference;
                if (m.sensorLastDatas.Count > 0)
                {
                    Difference = DateTime.Now - m.sensorLastDatas[0].DateTime;
                    if (Difference.Days > 3)
                        m.Icon = "/StationTypeImage/" + item.StationType.OfflineIcon;
                    else
                        m.Icon = "/StationTypeImage/" + item.StationType.OnlineIcon;
                }
                else
                    m.Icon = "/StationTypeImage/" + item.StationType.OfflineIcon;
                model.Add(m);
            }

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
