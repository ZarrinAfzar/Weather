using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.UnitOfWork;
using Weather.Data.ViewModel;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,Upload")]
    public class UploadController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly IHostingEnvironment _environment;
        private readonly UserManager<User> _userManager;

        public UploadController(GenericUoW genericUoW, IHostingEnvironment environment, UserManager<User> userManager)
        {
            _genericUoW = genericUoW;
            _environment = environment;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(int? id)
        {
            var model = new UploadVm
            {
                FilesList = _genericUoW.Repository<Files>().GetAll().ToList(),
                Files = id!=null ?_genericUoW.Repository<Files>().GetById(id.Value): new Files()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection form, UploadVm model)
        {

            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (form.Files != null && form.Files[0].Length > 0)
            {
                var webRoot = _environment.WebRootPath;
                if (!Directory.Exists(webRoot + "/UploadFiles/"))
                {
                    Directory.CreateDirectory(webRoot + "/UploadFiles/");
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFiles/", form.Files[0].FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    form.Files[0].CopyTo(stream);
                }
            }

            if (model.Files.Id != 0)
            { 
                var oldmodel = _genericUoW.Repository<Files>().GetById(model.Files.Id);
                model.Files.File = string.IsNullOrEmpty(form.Files[0].FileName) ? oldmodel.File : form.Files[0].FileName;
                _genericUoW.Repository<Files>().Update(model.Files, oldmodel);
                _genericUoW.Save(user.Id, EnuAction.Update, "آپلود فایل");
            }
            else
            {
                model.Files.File = form.Files[0].FileName;
                _genericUoW.Repository<Files>().Insert(model.Files);
                _genericUoW.Save(user.Id,EnuAction.Create,"آپلود فایل");
            }

            model = new UploadVm
            {
                FilesList = _genericUoW.Repository<Files>().GetAll().ToList(),
                Files = model.Files
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<Files>().Delete(id);
            _genericUoW.Save(user.Id, EnuAction.Create, "آپلود فایل");
            var model = new UploadVm
            {
                FilesList = _genericUoW.Repository<Files>().GetAll().ToList(),
                Files = new Files()
            };
            return View("Index",model);
        }
    }
}