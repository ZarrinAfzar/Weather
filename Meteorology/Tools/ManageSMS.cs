using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Base;
using Weather.Data.UnitOfWork;
using Weather.Models;
using wrmsms;

namespace Weather.Services
{
    public class ManageSMS
    {
        private readonly SMSwsdlPortType _smsClient;
        private readonly GenericUoW _uow;

        public ManageSMS(SMSwsdlPortType smsClient, GenericUoW uow)
        {
            _smsClient = smsClient;
            _uow = uow;
        }

        public async Task<bool> SendStartSMS(long? stationId, DateTime? startDate)
        {
            var stationName = _uow.Repository<Station>().GetAllQueryable(s => s.Id == stationId)
                .Select(s => s.Name).FirstOrDefault();

            var phones = string.Join(";", _uow.Repository<ManagerTel>()
                .GetAllQueryable(t => t.Start).Select(t => t.Tel));
            var StartDate = Convert.ToDateTime(startDate).ToPeString("yyyy/MM/dd HH:mm:ss");

            string text = $"شروع باران {stationName}\nزمان شروع: {StartDate}";
            return await SendSmsAsync(text, phones);
        }

        public async Task<bool> SendEndSMS(long? stationId, DateTime? startDate, DateTime? endDate)
        {
            var stationName = _uow.Repository<Station>().GetAllQueryable(s => s.Id == stationId)
                .Select(s => s.Name).FirstOrDefault();
            var volume = _uow.Repository<RainfallEvent>().GetAllQueryable(v => v.StationId == stationId)
               .Select(v => v.RainfallVolume).FirstOrDefault();
            var phones = string.Join(";", _uow.Repository<ManagerTel>()
                .GetAllQueryable(t => t.Start).Select(t => t.Tel));
            var StartDate = Convert.ToDateTime(startDate).ToPeString("yyyy/MM/dd HH:mm:ss");
            var EndDate = Convert.ToDateTime(endDate).ToPeString("yyyy/MM/dd HH:mm:ss");



            string text = $"پایان باران {stationName}\nشروع باران : {StartDate} \nپایان باران : {EndDate} \nمجموع بارندگی : {volume}";
            return await SendSmsAsync(text, phones);
        }

        private async Task<bool> SendSmsAsync(string text, string phoneNumbers)
        {
            try
            {

                string domain = "wrm-sms.ir";
                string username = "yzrw";
                string password = "yzrw97$";
                string from = "1000602336";
                string to = phoneNumbers;

                int isflash = 2;

                var result = await _smsClient.sendSMSAsync(domain, username, password, from, to, text, isflash);

                if (int.TryParse(result, out int code))
                {
                    switch (code)
                    {
                        case > 0:
                            Console.WriteLine($"ارسال موفق - شناسه پیامک: {code}");
                            return true;
                        case -1:
                            Console.WriteLine("❌ خطا در ارسال پیام");
                            break;
                        case -2:
                            Console.WriteLine("❌ نام کاربری یا کلمه عبور اشتباه است");
                            break;
                        case -3:
                            Console.WriteLine("❌ شماره فرستنده معتبر نیست");
                            break;
                        case -4:
                            Console.WriteLine("❌ اعتبار کافی نیست");
                            break;
                        case -5:
                            Console.WriteLine("⚠️ پیام در انتظار تأیید است");
                            break;
                        default:
                            Console.WriteLine($"❌ کد خطای ناشناخته ({code})");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("❌ پاسخ دریافتی معتبر نیست.");
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ خطا در ارسال پیامک: " + ex.Message);
                return false;
            }
        }
    }
}
