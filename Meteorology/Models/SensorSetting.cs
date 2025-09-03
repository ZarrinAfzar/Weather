using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// تنظیمات سنسورها
    /// </summary>
    public class SensorSetting: BaseModel
    {
        /// <summary>
        /// کلید خارجی ایستگاه
        /// </summary>
        [ForeignKey("Station")]
        public long StationId { get; set; }
        /// <summary>
        /// وضعیت سنسور
        /// </summary>
        public bool SensorEnable { get; set; }
        /// <summary>
        /// نوع سنسور مجازی =2 حقیقی =1
        /// </summary>
        public int SensorType { get; set; }
        /// <summary>
        /// سطر سنسور
        /// </summary>
        public int SensorRow { get; set; }
        /// <summary>
        /// کلید خارجی جدول سنسور
        /// </summary>
        [ForeignKey("SensorTypes")]
        public long? SensorTypeId { get; set; }
        /// <summary>
        /// نام سنسور
        /// </summary>
        public string SensorName { get; set; }
        /// <summary>
        /// واحد
        /// </summary>
        [ForeignKey("Unit")]
        public long? UnitId { get; set; }
        /// <summary>
        /// تعداد اعشار
        /// </summary>
        public int? SensorDigit{ get; set; } 
        /// <summary>
        /// کمترین مقدار
        /// </summary>
        public double? SensorMin { get; set; }
        /// <summary>
        /// بیشترین مقدار
        /// </summary>
        public double? SensorMax { get; set; }
        /// <summary>
        ///شماره سریال سنسور
        /// </summary>
        public string SensorSerial { get; set; }
        /// <summary>
        ///شرکت سازنده
        /// </summary>
        public string SensorCompany { get; set; }
        /// <summary>
        ///نوع تکنیکی سنسور
        /// </summary>
        public string  SensorTecnicalType{ get; set; }
        /// <summary>
        /// تاریخ
        /// </summary>
        public  DateTime SensorDateTime { get; set; }



        //-----------------------------------------------------------
        public virtual Station Station { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual SensorType SensorTypes { get; set; } 
        public virtual ICollection<SensorDateTime> SensorDateTimes { get; set; }


    }
}
