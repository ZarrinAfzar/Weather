using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,Correspondence")]
    public class CorrespondenceController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;

        public CorrespondenceController(GenericUoW genericUoW, UserManager<User> userManager)
        { 
            _genericUoW = genericUoW;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            List<Correspondence> correspondences = _genericUoW.Repository<Correspondence>().GetAll(u => u.UserSenderId == user.Id, u => u.User).ToList();
            List<Correspondence> Answer = _genericUoW.Repository<Correspondence>().GetAll(m=> correspondences.Any(x => x.Id == m.MessageAnswerId) || m.UserSenderId == user.Id, m => m.User).ToList();
                       return View(Answer);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string messageText, int UserSenderId)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            var model = new Correspondence()
            {
                MessageText = messageText,
                UserSenderId = UserSenderId,
                ViewState = false
            };
            _genericUoW.Repository<Correspondence>().Insert(model);
            _genericUoW.Save(user.Id, EnuAction.Update, "انتقادات و پیشنهادات");
            List<Correspondence> correspondences = _genericUoW.Repository<Correspondence>().GetAll(u => u.UserSenderId == user.Id, u => u.User).ToList();
            List<Correspondence> Answer = correspondences.Where(m => m.MessageAnswerId != null).ToList();
            List<Correspondence> Allcorrespondences = _genericUoW.Repository<Correspondence>().GetAll(m => Answer.Any(x => x.MessageAnswerId == m.Id), m => m.User).ToList();
            return View(Allcorrespondences);
        }
    }
}