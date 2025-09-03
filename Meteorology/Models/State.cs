using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// استان
    /// </summary>
    public class State : BaseModel
    { 
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; set; }

        //------------------------------------------------------
        public ICollection<City> City { get; set; } 

    }
}
