using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// جزئیات سنسور های مجازی
    /// </summary>
    public class VirtualSensorDetail : BaseModel
    {
        /// <summary>
        /// عنوا
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// کلید خارجی نوع سنسور
        /// </summary>
        [ForeignKey("Station")]
        public long StationId { get; set; } 
        /// <summary>
        /// نام پارامتر 1
        /// </summary>
        public string ParameterName1 { get; set; }
        /// <summary>
        /// نوع پارامتر 1
        /// </summary>
        public string ParameterType1 { get; set; }
        /// <summary>
        /// مقدار پارامتر 1
        /// </summary>
        public string ParameterValue1 { get; set; }
        /// <summary>
        /// نام پارامتر 2
        /// </summary>
        public string ParameterName2 { get; set; }
        /// <summary>
        /// نوع پارامتر 2
        /// </summary>
        public string ParameterType2 { get; set; }
        /// <summary>
        /// مقدار پارامتر 2
        /// </summary>
        public string ParameterValue2 { get; set; }
        /// <summary>
        /// نام پارامتر 3
        /// </summary>
        public string ParameterName3 { get; set; }
        /// <summary>
        /// نوع پارامتر 3
        /// </summary>
        public string ParameterType3 { get; set; }
        /// <summary>
        /// مقدار پارامتر 3
        /// </summary>
        public string ParameterValue3 { get; set; }
        /// <summary>
        /// نام پارامتر 4
        /// </summary>
        public string ParameterName4 { get; set; }
        /// <summary>
        /// نوع پارامتر 4
        /// </summary>
        public string ParameterType4 { get; set; }
        /// <summary>
        /// مقدار پارامتر 4
        /// </summary>
        public string ParameterValue4 { get; set; }
        /// <summary>
        /// نام پارامتر 5
        /// </summary>
        public string ParameterName5 { get; set; }
        /// <summary>
        /// نوع پارامتر 5
        /// </summary>
        public string ParameterType5 { get; set; }
        /// <summary>
        /// مقدار پارامتر 5
        /// </summary>
        public string ParameterValue5 { get; set; }

        //-----------------------------------------------------------
        public virtual Station Station { get; set; }
    }
}
