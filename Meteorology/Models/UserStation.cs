using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// ایستگاه های کاربر
    /// </summary>
    public class UserStation : BaseModel
    { 
        /// <summary>
        /// کد کاربر
        /// </summary>
        [ForeignKey("User")]
        public long UserId { get; set; }
        /// <summary>
        /// کد پروژه
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// کد ایستگاه
        /// </summary>
        public long StationId { get; set; }

        //------------------------------------------------------------
        public virtual User User { get; set; }
    } 
}
