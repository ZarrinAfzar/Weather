using Weather.Data.Base;
using Weather.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// ثبت اطلاعات پایه آلارم ها
    /// </summary>
    public class Alarm : BaseModel
    {
        /// <summary>
        /// کلید خارجی جدول ایستگاه ها
        /// </summary>
        [ForeignKey("Station")]
        public long StationId { get; set; }
        /// <summary>
        /// انواع آلارم (پیش بینی _ آفات _سنسور
        /// </summary>
        public EnuAlarm  AlarmType { get; set; }
        /// <summary>
        /// عنوان آلارم
        /// </summary>
        public string AlarmName { get; set; }
        /// <summary>
        /// متن پیامک
        /// </summary>
        public string AlarmMessage { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime AlarmStartDate { get; set; }
        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime AlarmEndDate { get; set; }
       
        //------------------------------------------------------------
        [NotMapped]
        public string AlarmStartDateConvertor
        {
            get
            {
                return AlarmStartDate != default(DateTime) ? AlarmStartDate.ToPeString() : "";
            }
            set
            {
                this.AlarmStartDate = value.ToGreDateTime();
            }
        }
        [NotMapped]
        public string AlarmEndDateConvertor
        {
            get
            {
                return AlarmEndDate != default(DateTime) ? AlarmEndDate.ToPeString() :"";
            }
            set
            {
                this.AlarmEndDate = value.ToGreDateTime();
            }
        }
        [NotMapped]
        public string AlarmColor
        {
            get
            {
                //if(AlarmType == EnuAlarm.Forecasts)
                //{
                //    return "text-success";
                //}
                 if (AlarmType == EnuAlarm.Pest)
                {
                    return "text-primary";
                }
                else if (AlarmType == EnuAlarm.Sensors)
                {
                    return "text-danger";
                }
                return "text-muted";
            }
        }
        //------------------------------------------------------------ 
        public virtual Station Station { get; set; } 
        public virtual ICollection<AlarmTell> AlarmTells { get; set; }
        public virtual ICollection<SmsSend> SmsSends { get; set; }
        public virtual ICollection<ForecastsAlarmDetail> ForecastsAlarmDetails { get; set; } 
        public virtual ICollection<SensorAlarmDetail> SensorAlarmDetails  { get; set; } 
        public virtual PestAlarmDetail PestAlarmDetail { get; set; } 
        public virtual ICollection<AlarmLog> AlarmLogs { get; set; }
    }
}
