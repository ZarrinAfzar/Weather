using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,AdminCorrespondence")]
    public class AdminCorrespondenceController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;

        public AdminCorrespondenceController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _genericUoW = genericUoW;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Index()
        {
            CorrespondenceViewModel model = new CorrespondenceViewModel()
            {
                CorrespondenceList = _genericUoW.Repository<Correspondence>().GetAll(null,n=>n.User),
                Correspondence = new Correspondence()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var rowselected = _genericUoW.Repository<Correspondence>().GetAll(n => n.Id == id, n => n.User).First();
            Correspondence AnswerCorrespondence = new Correspondence()
            {
                OldTxt = rowselected.MessageText,
                MessageText = "",
                MessageAnswerId = rowselected.Id,
                Id = 0,
            };
            CorrespondenceViewModel model = new CorrespondenceViewModel()
            {
                Correspondence = AnswerCorrespondence,
                CorrespondenceList = _genericUoW.Repository<Correspondence>().GetAll(null, n => n.User)
            };
            return View("Index",model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CorrespondenceViewModel model)
        {
            Correspondence question = _genericUoW.Repository<Correspondence>().GetById(model.Correspondence.MessageAnswerId.Value);
            question.ViewState = true;
            _genericUoW.Repository<Correspondence>().Update(question);
            _genericUoW.Repository<Correspondence>().Insert(model.Correspondence);
            User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Save(user.Id, EnuAction.Create, "انتقادات و پیشنهادات");
            CorrespondenceViewModel modelpassed = new CorrespondenceViewModel()
            {
                CorrespondenceList = _genericUoW.Repository<Correspondence>().GetAll(null, n => n.User),
                Correspondence = new Correspondence()
            };
            return View("Index", modelpassed);
        }


        [HttpGet]
        public async Task<IActionResult> changestate(long id) 
        {
            Correspondence question = _genericUoW.Repository<Correspondence>().GetById(id);
            question.ViewState = true;
            _genericUoW.Repository<Correspondence>().Update(question);
             User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Save(user.Id, EnuAction.Update, "انتقادات و پیشنهادات");
            CorrespondenceViewModel model = new CorrespondenceViewModel()
            {
                CorrespondenceList = _genericUoW.Repository<Correspondence>().GetAll(null, n => n.User),
                Correspondence = new Correspondence()
            };
            return View("Index", model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
             User user =await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<Correspondence>().Delete(id);
            _genericUoW.Save(user.Id, EnuAction.Delete, "انتقادات و پیشنهادات");
            CorrespondenceViewModel model = new CorrespondenceViewModel()
            {
                CorrespondenceList = _genericUoW.Repository<Correspondence>().GetAll(null, n => n.User),
                Correspondence = new Correspondence()
            };
            return View("Index", model);
        }
    }
}