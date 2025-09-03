using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class WindSpeedChartViewModel
    {
        public List<SpeedChartViewModel> SpeedChartViewModels { get; set; }
        public List<WindChartViewModel> WindChartViewModels { get; set; } 
    }
}
