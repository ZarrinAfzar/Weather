using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,UserSystemAction")]
    public class LogController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;

        public LogController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _genericUoW = genericUoW;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "AdminiStratore"))
            {
                LogViewModel model = new LogViewModel()
                {
                    UserActions = _genericUoW.Repository<UserAction>().GetAll(null, n => n.User).OrderByDescending(n => n.InsertDate).ToList(),
                    UserLoginHistorys = _genericUoW.Repository<UserLoginHistory>().GetAll(null, n => n.User).OrderByDescending(n => n.InsertDate).ToList(),
                };
                return View(model);
            }
            else
            {
                List<long> userthisInserted = _genericUoW.Repository<User>().GetAll(n => n.UserInsertedId == user.Id).Select(n => n.Id).ToList();
                userthisInserted.Add(user.Id);
                LogViewModel model = new LogViewModel()
                {
                    UserActions = _genericUoW.Repository<UserAction>().GetAll(n => userthisInserted.Any(c => c == n.UserId), n => n.User).OrderByDescending(n=>n.InsertDate).ToList(),
                    UserLoginHistorys = _genericUoW.Repository<UserLoginHistory>().GetAll(n => userthisInserted.Any(c => c == n.UserId), n => n.User).OrderByDescending(n => n.InsertDate).ToList(),
                };
                return View(model);
            }
        }
    }
}