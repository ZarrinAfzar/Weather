using Weather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class SensorReportViewModel
    { 
        public List<SensorSetting> SensorSettings { get; set; } 
        public List<SensorDateTime> SensorDateTimes { get; set; } 

    }
}
