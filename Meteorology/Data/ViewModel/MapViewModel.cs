using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class MapViewModel
    {
        public string StationName { get; set; }
        public string Latitude { get; set; } 
        public string Longitude { get; set; }
        public string Icon { get; set; }
        public string DateTime { get; set; }
        public List<sensorLastData> sensorLastDatas { get; set; }

    }
    public class sensorLastData 
    {
        public long StationId { get; set; }
        public string FaName { get; set; }
        public DateTime DateTime { get; set; }
        public double Data { get; set; }
    }
}
