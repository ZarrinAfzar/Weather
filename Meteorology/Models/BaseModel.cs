using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    public abstract class BaseModel
    {
       // [Column(TypeName = "bigint")]
        public long Id { get; set; }
        //public DateTime InsertDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}
