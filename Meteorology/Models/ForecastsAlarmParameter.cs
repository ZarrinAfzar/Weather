using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// پارامتر های آلارم های پیش بینی
    /// </summary>
    public class ForecastsAlarmParameter:BaseModel
    {
        /// <summary>
        /// عنوان
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// آیکون
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// مقدار
        /// </summary>
        public string Value { get; set; }
        [NotMapped]
        public string IconPath => string.IsNullOrEmpty(Icon) ? "/images/stationTypeIcon.png" : "/ForecastsAlarmParameterImage/" + Icon;
    }
}
