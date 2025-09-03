using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class SensorDataViewModel
    {
        public DateTime Date { get; set; } 
        public double Avg { get; set; }
        public double Sum { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
