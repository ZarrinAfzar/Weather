using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// شارژ پیامک پروژه
    /// </summary>
    public class SmsCharge : BaseModel
    {  
        /// <summary>
        /// کلید خارجی پروژه
        /// </summary>
        [ForeignKey("Project")]
        public long ProjectId { get; set; }
        /// <summary>
        /// تعداد پیامک
        /// </summary>
        public int Count { get; set; }
        public DateTime InsertDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));


        //-------------------------------------------------------
        public virtual Project Project{ get; set; }
    }
}
