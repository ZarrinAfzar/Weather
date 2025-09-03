using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class SensorSettingViewModel
    {
        public long Id { get; set; }
        public long SensorType { get; set; }
        public int Digit { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }

        public long? UnitId { get; set; }
      
        public string SensorSerial { get; set; }
        
        public string SensorCompany { get; set; }
        
        public string SensorTecnicalType { get; set; }
    }
}
