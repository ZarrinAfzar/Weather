using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// مقادیر سنسورها
    /// </summary>
    public class SensorDateTime : BaseModel
    {
        /// <summary>
        /// کلید خارجی جدول سنسورها
        /// </summary>
        [ForeignKey("SensorSetting")]
        public int SensorSettingId { get; set; }
        /// <summary>
        /// تاریخ
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// مقدار
        /// </summary>
        public double Data { get; set; }
        //------------------------------------------------------------
        public virtual SensorSetting SensorSetting { get; set; }
        //public virtual ICollection<SensorData> SensorDatas { get; set; }
    }
}
