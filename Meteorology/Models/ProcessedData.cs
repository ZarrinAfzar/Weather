using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather.Models
{
    /// <summary>
    /// وضعیت بارش برای هر سنسور
    /// </summary>
    public class ProcessedData : BaseModel
    {
        /// <summary>
        /// کلید خارجی جدول سنسورها
        /// </summary>
        [ForeignKey(nameof(SensorSetting))]
        public int SensorSettingId { get; set; }

        /// <summary>
        /// آخرین آیدی پردازش شده
        /// </summary>
        public long? LastProcessedId { get; set; }

        /// <summary>
        /// مقدار داده
        /// </summary>
        public double? DataValue { get; set; }
        //-----------------------------------------------------------
        /// <summary>
        /// ناوبری به تنظیمات سنسور
        /// </summary>
        public virtual SensorSetting SensorSetting { get; set; }
    }
}