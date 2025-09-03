using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// انتقادات و پیشنهادات
    /// </summary>
    public class Correspondence : BaseModel
    {
        public DateTime InsertDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        /// <summary>
        /// کلید پیام مورد پاسخ
        /// </summary>
        public long? MessageAnswerId { get; set; } 
        /// <summary>
        /// متن پیام
        /// </summary>
        public string MessageText { get; set; }
        /// <summary>
        /// کلید خارجی جدول کاربران
        /// </summary>
        [ForeignKey("User")]
        public long UserSenderId { get; set; }
        /// <summary>
        /// وضعیت بازدید پیام
        /// </summary>
        public bool ViewState { get; set; } = false;


        //--------------------------------------------------
        public virtual  User User{ get; set; }


        //--------------------------------------------------
        [NotMapped]
        public string OldTxt { get; set; }
    }
}
