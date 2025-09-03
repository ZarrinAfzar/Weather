using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// تلفن های ایستگاه
    /// </summary>
    public class StationTel : BaseModel
    { 
        /// <summary>
        /// کد ایستگاه
        /// </summary>
        [ForeignKey("Station")]
        public long StationId { get; set; }
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// پست
        /// </summary>
        public string Post { get; set; }
        /// <summary>
        /// تلفن
        /// </summary>
        public string Tel { get; set; }

        //-------------------------------------------------------
        public virtual Station Station { get; set; }
    }
}
