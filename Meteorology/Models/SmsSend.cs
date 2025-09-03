using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{/// <summary>
 /// جزئیات سنسور های مجازی
 /// </summary>
    public class SmsSend : BaseModel
    {
        /// <summary>  
        /// شماره تلفن جهت ارسال پیامک
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// متن پیامک
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime SendDate { get; set; }
       
        /// <summary>
        /// کاربر فعال سازی
        /// </summary>
        [ForeignKey("User")]
        public long? UserId { get; set; }
        public string Status { get; set; }
        public virtual User User { get; set; }
        public virtual Alarm Alarm { get; set; }
        
    }
}
