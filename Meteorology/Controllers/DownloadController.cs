using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,Download")]
    public class DownloadController : Controller
    {
        private readonly GenericUoW _genericUoW;

        public DownloadController(GenericUoW genericUoW)
        {
            _genericUoW = genericUoW;
        }
         
        public IActionResult Index()
        {
            return View(_genericUoW.Repository<Files>().GetAll().ToList());
        }
    }
}