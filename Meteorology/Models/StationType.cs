using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// انواع ایستگاه
    /// </summary>
    public class StationType : BaseModel
    { 
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///آنلاین آیکون
        /// </summary>
        public string OnlineIcon { get; set; }
        /// <summary>
        /// آیکون آفلاین
        /// </summary>
        public string OfflineIcon { get; set; }

        //---------------------------------------------------------
        public ICollection<Station> Stations { get; set; }
    }
}
