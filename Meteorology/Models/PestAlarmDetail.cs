using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// آلارم آفات
    /// </summary>
    public class PestAlarmDetail : BaseModel
    {
        /// <summary>
        /// کلید خارجی جدول آلارم
        /// </summary>
        [ForeignKey("Alarm")]
        public long AlarmId { get; set; }
        /// <summary>
        /// نام مرحله اول
        /// </summary>
        public string LevelName1 { get; set; }
        /// <summary>
        /// نام مرحله دوم
        /// </summary>
        public string LevelName2 { get; set; }
        /// <summary>
        /// نام مرحله سوم
        /// </summary>
        public string LevelName3 { get; set; }
        /// <summary>
        /// نام مرحله چهارم
        /// </summary>
        public string LevelName4 { get; set; }
        /// <summary>
        /// نام مرحله پنجم
        /// </summary>
        public string LevelName5 { get; set; }
        /// <summary>
        /// مقدار GDD 2
        /// </summary>
        public double? PDDLevel2 { get; set; }
        /// <summary>
        /// مقدار GDD 3
        /// </summary>
        public double? PDDLevel3 { get; set; }
        /// <summary>
        /// مقدار GDD 4
        /// </summary>
        public double? PDDLevel4 { get; set; }
        /// <summary>
        /// مقدار GDD 5
        /// </summary>
        public double? PDDLevel5 { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime? DateLevel2 { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime? DateLevel3 { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime? DateLevel4 { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime? DateLevel5 { get; set; }
        /// <summary>
        /// دمای آستانه پایه
        /// </summary>
        public double? PDDDate2 { get; set; }
        /// <summary>
        /// دمای آستانه پایه
        /// </summary>
        public double? PDDDate3 { get; set; }
        /// <summary>
        /// دمای آستانه پایه
        /// </summary>
        public double? PDDDate4 { get; set; }
        /// <summary>
        /// دمای آستانه پایه
        /// </summary>
        public double? PDDDate5 { get; set; }
        /// <summary>
        /// مرحله فعلی
        /// </summary>
        public int LevelNow { get; set; }
        /// <summary>
        /// تعداد مراحل رشد
        /// </summary>
        public int? CountLevel { get; set; }
        /// <summary>
        /// دمای آستانه پایه
        /// </summary>
        public double? TempBase { get; set; }
        /// <summary>
        /// دمای آستانه حداکثر
        /// </summary>
        public double? TempMax { get; set; }
      

        //------------------------------------------------------------

        public virtual Alarm Alarm { get; set; }
       
      
    }
   
}


