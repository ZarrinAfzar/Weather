using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// سنسورهای مجازی پایه
    /// </summary>
    public class VirtualSensorBase : BaseModel
    {
        /// <summary>
        /// کلید خارجی نوع سنسور
        /// </summary>
        [ForeignKey("SensorType")]
        public long SensorTypeId { get; set; }
        /// <summary>
        /// نام پارامتر 1
        /// </summary>
        public string ParameterName1 { get; set; }
        /// <summary>
        /// نوع پارامتر 1
        /// </summary>
        public string ParameterType1 { get; set; }
        [NotMapped]
        public string ParameterType1Title
        {
            get
            {
                if (this.ParameterType1 == "real")
                    return "عددی";
                if (this.ParameterType1 == "time")
                    return "ساعت";
                if (this.ParameterType1 == "date")
                    return "تاریخ";
                else
                    return "-";
            }
        }
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
        [NotMapped]
        public string ParameterType2Title
        {
            get
            {
                if (this.ParameterType2 == "real")
                    return "عددی";   
                if (this.ParameterType2 == "time")
                    return "ساعت";   
                if (this.ParameterType2 == "date")
                    return "تاریخ";
                else
                    return "-";
            }
        }
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
        [NotMapped]
        public string ParameterType3Title
        {
            get
            {
                if (this.ParameterType3 == "real")
                    return "عددی";   
                if (this.ParameterType3 == "time")
                    return "ساعت";   
                if (this.ParameterType3 == "date")
                    return "تاریخ";
                else
                    return "-";
            }
        }
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
        [NotMapped]
        public string ParameterType4Title
        {
            get
            {
                if (this.ParameterType4 == "real")
                    return "عددی";   
                if (this.ParameterType4 == "time")
                    return "ساعت";    
                if (this.ParameterType4 == "date")
                    return "تاریخ";
                else
                    return "-";
            }
        }
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
        [NotMapped]
        public string ParameterType5Title
        {
            get
            {
                if (this.ParameterType5 == "real")
                    return "عددی";    
                if (this.ParameterType5 == "time")
                    return "ساعت";   
                if (this.ParameterType5 == "date")
                    return "تاریخ";
                else
                    return "-";
            }
        }
        //-----------------------------------------------------------
        public virtual SensorType SensorType { get; set; }
    }
}
