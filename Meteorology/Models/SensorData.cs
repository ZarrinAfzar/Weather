using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Meteorology.Models
{
    public class SensorData:BaseModel
    {
        [ForeignKey("SensorDateTime")] 
        public int SensorDateTimeId { get; set; }
        [ForeignKey("SensorType")]
        public int SensorTypeId { get; set; }
        public double Data { get; set; }
        //------------------------------------------------------------
        public virtual SensorType SensorType { get; set; } 
        public virtual SensorDateTime SensorDateTime { get; set; }
       
    }
}
