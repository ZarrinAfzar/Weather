using Weather.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// تاریخچه ورود و خروج کاربر
    /// </summary>
    public class UserLoginHistory:BaseModel
    {
        /// <summary>
        /// کد کاربر
        /// </summary>
        [ForeignKey("User")]
        public long UserId { get; set; }
        /// <summary>
        /// نوع ورود یا خروج
        /// </summary>
        public EnuLoginHistory Type { get; set; }
        public DateTime InsertDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));


        //--------------------------------------------------------------
        public virtual User User { get; set; }
    }
}
