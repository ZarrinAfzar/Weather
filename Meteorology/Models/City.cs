using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// اطلاعات پایه - شهرها
    /// </summary>
    public class City: BaseModel
    { 

        /// <summary>
        /// کلید خارجی جدول استان ها
        /// </summary>
        [ForeignKey("State")]
        public long StateId { get; set; }
        /// <summary>
        /// نام شهر
        /// </summary>
        public string Name { get; set; }
         


        //------------------------------------------------------------
        public virtual State State{ get; set; }
        public ICollection<User> Users { get; set; }
    }
}
