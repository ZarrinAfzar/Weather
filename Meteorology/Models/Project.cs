using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Base;

namespace Weather.Models
{
    /// <summary>
    /// جدول پروژه ها
    /// </summary>
    public class Project : BaseModel
    {
        public DateTime InsertDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        /// <summary>
        /// نام پروژه
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// کد پروژه
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// تعداد پیامک
        /// </summary>
        public int SmsCount { get; set; } = 0;
        /// <summary>
        /// مدیر پروژه
        /// </summary>
        //public long? UserId { get; set; }

        //---------------------------------------------------
        //public virtual ICollection<UserStation> UserStations { get; set; }
        public virtual ICollection<Station> Stations { get; set; }
        //public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<SmsCharge> SmsCharges { get; set; }
    }
}
