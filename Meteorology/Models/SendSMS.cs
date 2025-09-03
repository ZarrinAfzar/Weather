using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;

namespace Weather.Models
{
    public class SendSMS:BaseModel 
    {
        /// <summary>  
        /// تاریخ ارسال پیامک
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>  
        /// شماره تلفن جهت ارسال پیامک
        /// </summary>
        public string To { get; set; }
        /// <summary>  
        /// متن پیامک
        /// </summary>
        public string Message { get; set; }
        /// <summary>  
        /// شماره تلفن جهت ارسال پیامک
        /// </summary>
        public string Status { get; set; }
        /// <summary>  
        /// تاریخ شمسی ارسال پیامک
        /// </summary>
        public string DatePrs { get; set; }
        /// <summary>  
        /// نام متصدی
        /// </summary>
        public string Name { get; set; }
        /// <summary>  
        /// نوع پیامک
        /// </summary>
        public string Type { get; set; }
    }
}
