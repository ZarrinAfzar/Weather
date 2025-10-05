using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather.Models
{
    public class RainfallEvent : BaseModel
    {
        /// <summary>
        /// کلید خارجی جدول سنسورها
        /// </summary>
        [ForeignKey(nameof(SensorSetting))]
        public int SensorSettingId { get; set; }

        /// <summary>
        /// کلید خارجی جدول ایستگاه 
        /// </summary>
        [ForeignKey(nameof(Station))]
        public long? StationId { get; set; }

        /// <summary>
        /// تیپ ایستگاه 
        /// </summary>
        public int? StationType { get; set; }

        /// <summary>
        /// زمان آغاز بارش به میلادی
        /// </summary>
        public DateTime? RainStart { get; set; }

        /// <summary>
        /// زمان پایان بارش به میلادی
        /// </summary>
        public DateTime? RainEnd { get; set; }

        /// <summary>
        /// اولین ID که بارش داشتیم
        /// </summary>
        public long? FirstIdWithRain { get; set; }

        /// <summary>
        /// آخرین ID که بارش داشتیم
        /// </summary>
        public long? LastIdWithRain { get; set; }

        /// <summary>
        /// درحال بارش هست؟
        /// </summary>
        public bool IsRaining { get; set; }

        /// <summary>
        /// حجم بارندگی
        /// </summary>
        public double RainfallVolume { get; set; }

        /// <summary>
        /// پیامک آغاز ارسال شده یا نه؟
        /// </summary>
        public bool IsStartSMSSent { get; set; }

        /// <summary>
        /// پیامک پایان ارسال شده یا نه؟
        /// </summary>
        public bool IsEndSMSSent { get; set; }

        /// <summary>
        /// ناوبری به ایستگاه
        /// </summary>        
        public virtual Station Station { get; set; }
    }
}