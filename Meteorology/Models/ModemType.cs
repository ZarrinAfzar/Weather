using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// انواع مودم
    /// </summary>
    public class ModemType : BaseModel
    { 
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; }

        //---------------------------------------------------------
        public ICollection<Station> Stations { get; set; }
    }
}
