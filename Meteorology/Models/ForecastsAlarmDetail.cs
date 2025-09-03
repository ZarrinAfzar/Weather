using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// جدول آلارم های پیش بینی
    /// </summary>
    public class ForecastsAlarmDetail : BaseModel
    {
        /// <summary>
        /// کلید خارجی آلارم
        /// </summary>
        [ForeignKey("Alarm")] 
        public long AlarmId { get; set; }
        /// <summary>
        /// کلید خارجی پارامتر های آلارم پیش بینی
        /// </summary>
        [ForeignKey("ForecastsAlarmParameter")]
        public long ForecastsAlarmParameterId { get; set; }
        /// <summary>
        /// کمترین یا بیشترین
        /// </summary>
        public bool MinOrMax { get; set; } = false;
        /// <summary>
        /// کمترین یا بیشترین مقدار
        /// </summary>
        public double? MinOrMaxvalue { get; set; }
        //------------------------------------------------------------
        public virtual ForecastsAlarmParameter ForecastsAlarmParameter { get; set; }
        public virtual Alarm Alarm { get; set; }
        public virtual ICollection<ForecastsLog> ForecastsLogs { get; set; }

    }
}
