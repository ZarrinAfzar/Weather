using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    public class StationCompare
    {/// <summary>
     /// کد ایستگاه
     /// </summary>
        [ForeignKey("Station")]
        public long StationId { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public string EndDate { get; set; }
       

        //-------------------------------------------------------
        public virtual Station Station { get; set; }

    }
}
