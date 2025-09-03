using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// آلارم سنسورها
    /// </summary>
    public class SensorAlarmDetail : BaseModel
    {
        /// <summary>
        /// کلید خارجی جدول آلارم
        /// </summary>
        [ForeignKey("Alarm")]
        public long AlarmId { get; set; }
        /// <summary>
        /// کلید خارجی جدول انواع سنسور
        /// </summary>
        [ForeignKey("SensorType")]
        public long SensorTypeId { get; set; }
        /// <summary>
        /// کمترین یا بیشترین
        /// </summary>
        public bool MinimumOrMaximum { get; set; } 
        /// <summary>
        /// کمترین یا بیشترین مقدار
        /// </summary>
        //[Column(TypeName = "double")]
        public double MinMaxValue { get; set; }
        /// <summary>
        /// فوری یا مدت دار
        /// </summary>
        public bool OnlineOrTimeDuration { get; set; }
        /// <summary>
        /// مدت فوری
        /// </summary>
        public int? TimeDuration { get; set; }

        /// <summary>
        /// آستانه بررسی مجدد
        /// </summary>
       public double? Threshold { get; set; }
        //------------------------------------------------------------

        public virtual Alarm Alarm { get; set; }
        public virtual SensorType SensorType { get; set; }
    }
}
