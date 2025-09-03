using Weather.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// انواع سنسور
    /// </summary>
    public class SensorType: BaseModel
    { 
        /// <summary>
        /// نام لاتین
        /// </summary>
        public string EnName { get; set; }
        /// <summary>
        /// نام فارسی
        /// </summary>
        public string FaName { get; set; }
        /// <summary>
        /// حقیقی یا حقوقی
        /// </summary>
        public EnuSensorType  SensorType_State { get; set; }
        /// <summary>
        /// واحد اندازه گیری
        /// </summary>
        [ForeignKey("Unit")]
        public long UnitId { get; set; }
        /// <summary>
        /// ارقام اعشار
        /// </summary>
        public int SensorDigit { get; set; }
        /// <summary>
        /// بیشترین مقدار
        /// </summary>
        public double SensorMax { get; set; }
               /// <summary>
        /// کمترین مقدار
        /// </summary>
        public double SensorMin { get; set; }
        /// <summary>
        /// میانگین دارد؟
        /// </summary>
        public bool AVG { get; set; }
        /// <summary>
        /// مینیموم دارد؟
        /// </summary>
        public bool Min { get; set; }
        /// <summary>
        /// ماکزیمم دارد؟
        /// </summary>
        public bool Max { get; set; }
        /// <summary>
        /// مجموع دارد؟
        /// </summary>
        public bool Sum { get; set; }
        /// <summary>
        /// سسنور سرعت است؟
        /// </summary>
        public bool WindSpeed { get; set; }
        /// <summary>
        /// سنسور جهت است؟
        /// </summary>
        public bool WindDirect { get; set; }
        /// <summary>
        /// سنسور محاسبه درجه روز رشد است؟
        /// </summary>
        public bool DegreeDay { get; set; }
        /// <summary>
        /// میانگین برداری دارد؟
        /// </summary>
        public bool AvgVect { get; set; }

        //-----------------------------------------------------------
        public virtual ICollection<SensorSetting> SensorSettings { get; set; }
        public virtual ICollection<SensorAlarmDetail> SensorAlarmDetails { get; set; }
        public virtual VirtualSensorBase VirtualSensorBase { get; set; }
        public virtual Unit Unit { get; set; }
        //-----------------------------------------------------------
        [NotMapped]
        public string ViewName => FaName + " ( " + EnName + " )";
    }
}
