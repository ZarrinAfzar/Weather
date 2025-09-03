using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// اطلاعات پایه - دیتالاگر ها
    /// </summary>
    public class DataLogger : BaseModel
    { 
        /// <summary>
        /// نام دیتالاگر
        /// </summary>
        public string Name { get; set; }

        //---------------------------------------------------------
        public ICollection<Station> Stations { get; set; }

    }
}
