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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Globalization;
using Weather.Data.Base;
using Microsoft.CodeAnalysis.Differencing;

namespace Weather.Controllers
{
    [Authorize(Roles = "AdminiStratore,SMSPanel")]
    public class SMSPanelController : Controller
    {
        private readonly GenericUoW _genericUoW;
        private readonly UserManager<User> _userManager;

        public SMSPanelController(GenericUoW genericUoW, UserManager<User> userManager)
        {
            _genericUoW = genericUoW;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            List<SendSMS> model = new List<SendSMS>();
            if ((await _userManager.IsInRoleAsync(user, "AdminiStratore")) || (await _userManager.IsInRoleAsync(user, "SMSPanel")))
            {

                ViewBag.TelList = new SelectList(_genericUoW.Repository<Station>().GetAll().Select(n => new { n.Id, n.Name }).OrderBy(m => m.Name).ToList(), "Id", "Name");
                model = _genericUoW.Repository<SendSMS>().GetAll(n => n.Type == "P").ToList();
            }

            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> SMSPanelMessage(int SMSPanelId)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            SmsSend SMSPanel = _genericUoW.Repository<SmsSend>().GetById(SMSPanelId);

            return Json(SMSPanel.Text);
        }

        [HttpPost]
        public async Task<JsonResult> SendMessage(int[] OperatorGeter, string Message)
        {
            JsonResult j = Json(false);
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (OperatorGeter.Contains(0))
            {
                List<Station> Operatorids = _genericUoW.Repository<Station>().GetAll(n => n.OperatorPhonNO != null).ToList();
                foreach (var item in Operatorids)
                {
                    _genericUoW.Repository<SendSMS>().Insert(SendSMSTO(Message, item.OperatorPhonNO, item.Name));
                    j = Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Create, "ارسال پیامک از پنل")));

                }
            }
            else if (OperatorGeter.Count() > 0)
            {
                for (int i = 0; i < OperatorGeter.Count(); i++)
                {
                    Station Operatorids = _genericUoW.Repository<Station>().GetById(OperatorGeter[i]);
                    SendSMS s = SendSMSTO(Message, Operatorids.OperatorPhonNO, Operatorids.Name);
                    _genericUoW.Repository<SendSMS>().Insert(s);
                    j = Json(Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Create, "ارسال پیامک از پنل")));

                }
            }

            return j;
        }

        [HttpGet]
        public async Task<IActionResult> SMSPanel_List(string OperatorNameSearch, string valueSearch, int sort = 0)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            var model = _genericUoW.Repository<SendSMS>().GetAll(n => n.Type == "P");

            if ((await _userManager.IsInRoleAsync(user, "AdminiStratore")) || (await _userManager.IsInRoleAsync(user, "SMSPanel")))
            {

                ViewBag.TelList = new SelectList(_genericUoW.Repository<Station>().GetAll().Select(n => new { n.Id, n.OperatorName }).OrderBy(m => m.OperatorName).ToList(), "Id", "OperatorName");

            }
            if (!string.IsNullOrEmpty(OperatorNameSearch))
            {

                model = model.FindAll(m => m.Name.Contains(OperatorNameSearch)).ToList();

            }
            if (!string.IsNullOrEmpty(valueSearch))
            {

                model = model.FindAll(m => m.Message.Contains(valueSearch)).ToList();

            }

            switch (sort)
            {

                case 3:
                    model = model.OrderBy(n => n.SendTime).ToList();
                    break;
                case 4:
                    model = model.OrderByDescending(n => n.SendTime).ToList();
                    break;
                case 5:
                    model = model.OrderBy(n => n.Name).ToList();
                    break;
                case 6:
                    model = model.OrderByDescending(n => n.Name).ToList();
                    break;
                default:
                    break;
            }
            return PartialView(model);
        }
        public SendSMS SendSMSTO(string SMS, string Mobile, string Name)
        {
            SendSMS sms = new SendSMS();
            try
            {
                WebRequest request = WebRequest.Create("https://ippanel.com/services.jspd");
                request.Method = "POST";
                string status = "";
                string json;
                request = WebRequest.Create("https://ippanel.com/services.jspd");
                json = JsonConvert.SerializeObject(Mobile);
                request.Method = "POST";
                string postData = "op=send&uname=u9131540446&pass=Aa123456@&message=" + SMS + "&to=" + json + "&from=500010703531077";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                // dataStream.Close();
                WebResponse response = request.GetResponse();
                // StreamReader reader = new StreamReader(dataStream);
                //string responseFromServer = reader.ReadToEnd();
                // reader.Close();
                dataStream.Close();
                response.Close();


                sms.Type = "P";
                sms.DatePrs = DateTime.Now.ToPeString("yyyy/MM/dd HH:mm");
                sms.SendTime = DateTime.Now;
                sms.Message = SMS;
                sms.To = Mobile;
                sms.Name = Name;


            }
            catch (Exception ex)
            {

            }
            return sms;
        }


        #region ManagerTel
        [HttpGet]
        public IActionResult ManagerTel()
        {
            List<ManagerTel> model = _genericUoW.Repository<ManagerTel>().GetAll().ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult ManagerTel_list()
        {
            List<ManagerTel> model = _genericUoW.Repository<ManagerTel>().GetAll().ToList();
            return PartialView("ManagerTel_list", model.ToList());
        }
        [HttpGet]
        public IActionResult ManagerTel_Get(int id)
        {

            ManagerTel model = _genericUoW.Repository<ManagerTel>().GetById(id);
            bool succsess = (model != null);
            return PartialView("ManagerTel_Get", model);
        }
        [HttpGet]
        public IActionResult ManagerTel_Create()
        {
            return PartialView("ManagerTel_Create");
        }

        [HttpPost]
        public async Task<JsonResult> ManagerTel_Create(string Name, string LastName, List<string> Type, string Tel)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new ManagerTel()
            {
                Name = Name,
                LastName = LastName,
                Start = Type != null && Type.Contains("Start"),
                End = Type != null && Type.Contains("End"),
                Sum12 = Type != null && Type.Contains("Sum12"),
                Sum24 = Type != null && Type.Contains("Sum24"),
                Warning = Type != null && Type.Contains("Warning"),
                Sum = Type != null && Type.Contains("Sum"),
                Accepted = Type != null && Type.Contains("Accepted"),
                NotAccepted = Type != null && Type.Contains("NotAccepted"),
                SumOprator = Type != null && Type.Contains("SumOprator"),
                AcceptEditInsert = Type != null && Type.Contains("AcceptEditInsert"),
                Tel = Tel
            };

            _genericUoW.Repository<ManagerTel>().Insert(model);

            bool saveResult = _genericUoW.Save(user.Id, EnuAction.Create, "تلفن های مدیران");

            return Json(new { success = saveResult, data = saveResult ? model : null });
        }


        [HttpPost]
        public async Task<IActionResult> ManagerTel_Edit(ManagerTel model)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            {
                ManagerTel old_ManagerTel = _genericUoW.Repository<ManagerTel>().GetById(model.Id);
                _genericUoW.Repository<ManagerTel>().Update(model, old_ManagerTel);
                bool saved = Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Update, ""));
                return Json(new { success = saved });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errors });
            }
        }
        public async Task<JsonResult> ManagerTel_Delete(int Id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            _genericUoW.Repository<ManagerTel>().Delete(Id);
            return Convert.ToBoolean(_genericUoW.Save(user.Id, EnuAction.Delete, "تلفن های ایستگاه")) ? Json(Id) : Json(false);
        }
        #endregion
    }
}