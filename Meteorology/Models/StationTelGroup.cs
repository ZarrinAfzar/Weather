using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    public class StationTelGroup : BaseModel
    {
        /// <summary>
        /// نام گروه تلفن
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// کلید خارجی جدول تلفن های آلارم ها
        /// </summary>
        [ForeignKey("StationTel")]
        public long StationTelId { get; set; }

        //------------------------------------------------------------

        public virtual StationTel StationTel { get; set; }
    }
}
