using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// واحد اندازه گیری
    /// </summary>
    public class Unit : BaseModel
    {
        /// <summary>
        /// نام لاتین
        /// </summary>
        public string EnName { get; set; } 
        /// <summary>
        /// نام فارسی
        /// </summary>
        public string FaName { get; set; }
        //-----------------------------------------------------------
        public virtual ICollection<SensorSetting> SensorSettings { get; set; }
    }
}
