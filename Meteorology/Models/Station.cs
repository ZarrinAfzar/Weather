using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Weather.Models
{
    /// <summary>
    /// ایستگاه ها
    /// </summary>
    public class Station : BaseModel
    { 
        /// <summary>
        /// نام ایستگاه
        /// </summary>
        public string Name { get; set; } 
        /// <summary>
        /// شماره سریال
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// کلید خارجی پروژه
        /// </summary>
        [ForeignKey("Project")]
        public long ProjectId { get; set; }
        /// <summary>
        /// کلید خارجی دیتالاگر
        /// </summary>
        [ForeignKey("DataLogger")]
        public long DataLoggerId { get; set; }
        /// <summary>
        /// کلید خارجی نوع ایستگاه
        /// </summary>
        [ForeignKey("StationType")]
        public long StationTypeId { get; set; }
        /// <summary>
        /// شماره ایستگاه
        /// </summary>
        public string StationCardNumber { get; set; }
        /// <summary>
        /// نوع مودم
        /// </summary>
        [ForeignKey("ModemType")]
        public long ModemTypeId { get; set; }
        /// <summary>
        /// محل 1
        /// </summary>
        public string LocationKeyOne { get; set; }
        /// <summary>
        /// محل 2
        /// </summary>
        public string LocationKeyTwo { get; set; }
        /// <summary>
        /// عرض جغرافیایی
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// طول جغرافیایی
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AboveSeaLevel { get; set; } 
        /// <summary>
        /// کد ایستگاه
        /// </summary>
        public string StationCode { get; set; }
        /// <summary>
        /// به روز رسانی  زمان ایستگاه
        /// </summary>
        public bool UpdateTime { get; set; } = false;
        /// <summary>
        /// به روز رسانی  تنظیمات ایستگاه
        /// </summary>
        public bool UpdateSetting { get; set; } = false;
        /// <summary>
        /// تعداد پیامک
        /// </summary>
        public int? SmsCount { get; set; } = 0;
        /// <summary>
        /// تاریخ شارژ
        /// </summary>
        public DateTime? ChargeSms { get; set; }
        ///// <summary>
        ///// تصویر ایستگاه
        ///// </summary>
       public string Image { get; set; }
        /// <summary>
        /// شماره متصدی
        /// </summary>
        public string OperatorPhonNO  { get; set; }
        /// <summary>
        /// نام متصدی
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// ضریب نیسن
        /// </summary>
        public double? Tisen { get; set; }


        //-----------------------------------------------------------
        public virtual Project Project { get; set; }
        public virtual DataLogger DataLogger { get; set; }
        public virtual StationType StationType { get; set; }
        public virtual ModemType ModemType { get; set; }
        public virtual ICollection<StationFile> StationFiles { get; set; } 
        public virtual ICollection<StationTel> StationTels { get; set; }
        public virtual ICollection<SensorSetting> SensorSettings { get; set; }
        public virtual ICollection<VirtualSensorDetail> VirtualSensorDetails  { get; set; }




    }
}
