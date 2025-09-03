using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// لاگ های آلارم های پیش بینی
    /// </summary>
    public class ForecastsLog:BaseModel
    {
        /// <summary>
        /// کلید خارجی جدول آلارم
        /// </summary>
        //[ForeignKey("Alarm")]
        public long AlarmId { get; set; }
        /// <summary>
        /// کلید خارجی جدول آلارم های پیش بینی
        /// </summary>
        [ForeignKey("ForecastsAlarmDetail")] 
        public long ForecastsAlarmDetailId { get; set; }
        /// <summary>
        /// کمترین یا بیشترین مقدار
        /// </summary>
        public double ForecastsMinOrMax { get; set; }
        /// <summary>
        /// متن 
        /// </summary>
        public string ForecastsText { get; set; }

        //------------------------------------------------------------
        //public virtual Alarm Alarm { get; set; }
        public virtual ForecastsAlarmDetail ForecastsAlarmDetail { get; set; }
    }
}
