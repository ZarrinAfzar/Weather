using Weather.Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// پیام های کاربر
    /// </summary>
    public class Notification : BaseModel
    {
        public DateTime InsertDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        /// <summary>
        /// کلید خارجی جدول کاربران
        /// </summary>
        [ForeignKey("UserRecive")]
       
        public long UserGetId { get; set; }
        /// <summary>
        /// وضعیت بازدید
        /// </summary>
        public bool ViewState { get; set; } 
        /// <summary>
        /// متن پیام
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// کاربر ارسال کننده
        /// </summary>
        [ForeignKey("UserSend")]
        
        public long UserInsertedId { get; set; }
        //--------------------------------------------------
        [InverseProperty("NotificationsRecive")]
        public virtual User UserRecive { get; set; }
        [InverseProperty("NotificationsSend")]
        public virtual User UserSend { get; set; }
    }
}
