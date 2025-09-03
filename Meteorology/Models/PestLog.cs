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
    public class PestLog:BaseModel
    {
        /// <summary>
        /// کلید خارجی آلارم
        /// </summary>
        [ForeignKey("Alarm")]
        public long AlarmId { get; set; }
        /// <summary>
        /// تاریخ رخ دادن آلارم
        /// </summary>
        public DateTime DateTime { get; set; } 
        /// <summary>
        /// مقدار درجه روز رشد
        /// </summary>
       public double PDD { get; set; }
        /// <summary>
        /// مرحله آفت
        /// </summary>
        public  string LevelName { get; set; }
        //------------------------------------------------------- 
        public virtual Alarm Alarm { get; set; }
    }
}
