using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// جدول گزارش از آلارم های ثبت شده
    /// </summary>
    public class AlarmLog:BaseModel
    {
        /// <summary>
        /// کلید خارجی آلارم
        /// </summary>
        [ForeignKey("Alarm")]
        public long AlarmId { get; set; }
        /// <summary>
        /// تاریخ رخ دادن آلارم
        /// </summary>
        public DateTime AlarmDateTime { get; set; } 
        /// <summary>
        /// متن پیام
        /// </summary>
        public string AlarmMessage { get; set; }

        /// <summary>
        /// تاریخ آلارم ثبت شده
        /// </summary>
        public DateTime InsertDateTime { get; set; }
        //------------------------------------------------------- 
        public virtual Alarm Alarm { get; set; }
    }
}
