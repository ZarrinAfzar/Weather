using Weather.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather.Models
{
    /// <summary>
    /// فعالیت های کاربر
    /// </summary>
    public class UserAction : BaseModel
    { 
        public DateTime InsertDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        /// <summary>
        /// کد کاربر
        /// </summary>
        [ForeignKey("User")]  
        public long UserId { get; set; }  
        /// <summary>
        /// نوع فعالیت
        /// </summary>
        public EnuAction ActionType { get; set; } 
        /// <summary>
        /// نام موجودیت
        /// </summary>
        public string EntityName { get; set; }

        //--------------------------------------------------------------
        public virtual User User { get; set; } 
    }   
}
