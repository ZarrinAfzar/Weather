using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Data.ViewModel
{
    public class BaseInfoVm
    {
        public List<State> States{ get; set; }
        public List<City> Cities{ get; set; }
        public List<DataLogger> DataLoggers{ get; set; }
        public List<StationType> StationTypes{ get; set; }
        public List<ModemType> ModemTypes{ get; set; } 
        public List<SensorType> SensorTypes { get; set; }
        public List<ForecastsAlarmParameter> ForecastsAlarmParameters { get; set; }
        public List<Unit> Units { get; set; }
        public List<VirtualSensorBase> VirtualSensorBases { get; set; }
    } 
}
