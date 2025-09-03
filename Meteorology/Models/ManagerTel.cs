using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// تلفن های مدیران
    /// </summary>
    public class ManagerTel : BaseModel
    {
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// نوع مدیریت
        /// </summary>
        public bool Start { get; set; }
        public bool End { get; set; }
        public bool Sum12 { get; set; }
        public bool Sum24 { get; set; }
        public bool Warning { get; set; }
        public bool Sum { get; set; }
        public bool Accepted { get; set; }
        public bool NotAccepted { get; set; }
        public bool SumOprator { get; set; }
        public bool AcceptEditInsert { get; set; }
        /// <summary>
        /// تلفن
        /// </summary>
        public string Tel { get; set; }


    }
}
