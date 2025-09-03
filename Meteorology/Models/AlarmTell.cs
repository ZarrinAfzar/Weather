using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// تلفن های آلارم
    /// </summary>
    public class AlarmTell : BaseModel
    {
        /// <summary>
        /// کلید خارجی جدول آلارم ها
        /// </summary>
        [ForeignKey("Alarm")]
        public long AlarmId { get; set; }

        /// <summary>
        /// کلید خارجی جدول ایستگاه ها
        /// </summary>
        [ForeignKey("StationTel")]
        public long StationTelId { get; set; } = 0;

        //------------------------------------------------------------
        public virtual Alarm Alarm { get; set; }
        public virtual StationTel StationTel { get; set; }
    }
}
