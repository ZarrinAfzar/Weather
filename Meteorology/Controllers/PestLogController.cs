using System;
using System.Collections.Generic;
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
    [Authorize(Roles = "AdminiStratore,PestLog")]
    public class PestLogController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;
        public PestLogController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _userManager = userManager;
            _genericUoW = genericUoW;
        }

        [HttpGet]
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

        [HttpGet]
        //public async Task<IActionResult> PestLog_List(string FromDate, string ToDate, int ProjectId = 0, int StationId = 0)
        //{
        //    User user = await _userManager.GetUserAsync(HttpContext.User);
        //    //List<PestLog> model = new List<PestLog>();
        //    //Expression<Func<PestLog, object>>[] join = { n => n.Alarm , x=>x.Alarm.Station , x=>x.Alarm.Station.Project };
        //    //Expression<Func<PestLog, bool>> where = null;


        ////    if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
        ////    {
        ////        if (ProjectId != 0 && StationId != 0)
        ////        {
        ////            //where = n => n.Alarm.Station.Project.Id == ProjectId && n.Alarm.Station.Id == StationId; 
        ////        }
        ////        else if (ProjectId != 0)
        ////        { 
        ////            //where = n => n.Alarm.Station.Project.Id == ProjectId;
        ////        }
        ////        else if (StationId != 0)
        ////        { 
        ////            //where = n => n.Alarm.Station.Id == StationId;
        ////        }

        ////        if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
        ////        {
        ////            DateTime fromdatetime = FromDate.ToGreDateTime();
        ////            DateTime toDatetime = ToDate.ToGreDateTime();
        ////            if(ProjectId != 0 || StationId != 0)
        ////            {
        ////                //var prefix = where.Compile();
        ////                //where = n => prefix(n) && n.DateTime > fromdatetime && n.DateTime < toDatetime;
        ////            }
        ////            else
        ////            {
        ////                where = n => n.DateTime > fromdatetime && n.DateTime < toDatetime;
        ////            }
        ////        }
        ////        else if (!string.IsNullOrEmpty(FromDate))
        ////        {
        ////            DateTime fromdatetime = FromDate.ToGreDateTime();
        ////            if (ProjectId != 0 || StationId != 0)
        ////            {
        ////                var prefix = where.Compile();
        ////                where = n => prefix(n) && n.DateTime > fromdatetime;
        ////            }
        ////            else
        ////            {
        ////                where = n => n.DateTime > fromdatetime;
        ////            }
                   
        ////        }
        ////        else if (!string.IsNullOrEmpty(ToDate))
        ////        {
        ////            DateTime toDatetime = ToDate.ToGreDateTime();
        ////            if (ProjectId != 0 || StationId != 0)
        ////            {
        ////                var prefix = where.Compile();
        ////                where = n => prefix(n) && n.DateTime < toDatetime;
        ////            }
        ////            else
        ////            {
        ////                where = n => n.DateTime < toDatetime;
        ////            }
                  
        ////        }
        ////    }
        ////    else
        ////    {
        ////        List<UserStation> StationUserId = _genericUoW.Repository<UserStation>().GetAll(n => n.UserId == user.Id).ToList();
        ////        if (ProjectId != 0 && StationId != 0)
        ////        {
        ////            where = n => n.Alarm.Station.Project.Id == ProjectId && StationUserId.Any(x=>x.StationId == StationId) && n.Alarm.Station.Id == StationId;
        ////        }
        ////        else if (ProjectId != 0)
        ////        {
        ////            where = n => n.Alarm.Station.Project.Id == ProjectId && StationUserId.Any(x => x.ProjectId == ProjectId);
        ////        }
        ////        else if (StationId != 0)
        ////        {
        ////            where = n => n.Alarm.Station.Id == StationId && StationUserId.Any(x => x.StationId == StationId);
        ////        }


        ////        if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
        ////        {
        ////            DateTime fromdatetime = FromDate.ToGreDateTime();
        ////            DateTime toDatetime = ToDate.ToGreDateTime();
        ////            if (ProjectId != 0 || StationId != 0)
        ////            {
        ////                var prefix = where.Compile();
        ////                where = n => prefix(n) && n.DateTime > fromdatetime && n.DateTime < toDatetime;
        ////            }
        ////            else
        ////            {
        ////                where = n => n.DateTime > fromdatetime && n.DateTime < toDatetime;
        ////            }

        ////        }
        ////        else if (!string.IsNullOrEmpty(FromDate))
        ////        {
        ////            DateTime fromdatetime = FromDate.ToGreDateTime();
        ////            if (ProjectId != 0 || StationId != 0)
        ////            {
        ////                var prefix = where.Compile();
        ////                where = n => prefix(n) && n.DateTime > fromdatetime;
        ////            }
        ////            else
        ////            {
        ////                where = n =>n.DateTime > fromdatetime;
        ////            }
        ////        }
        ////        else if (!string.IsNullOrEmpty(ToDate))
        ////        {
        ////            DateTime toDatetime = ToDate.ToGreDateTime();
        ////            if (ProjectId != 0 || StationId != 0)
        ////            {
        ////                var prefix = where.Compile();
        ////                where = n => prefix(n) && n.DateTime < toDatetime;
        ////            }
        ////            else
        ////            {
        ////                where = n => n.DateTime < toDatetime;
        ////            }

        ////        }
 
        ////    } 
        ////    return where != null ? PartialView(_genericUoW.Repository<PestLog>().GetAllByQuery(where.Compile(), join).ToList()) : PartialView(_genericUoW.Repository<PestLog>().GetAllByQuery(null, join).ToList());
        //}

        [HttpPost]
        public async Task<JsonResult> GetStation(int Projectid = 0)
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
    }
}