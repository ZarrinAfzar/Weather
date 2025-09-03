using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class StationSmsViewModel
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string ProjectName { get; set; }
        public int StationSmsCount { get; set; }
        public int ProjectSmsCount { get; set; }
        public int StationSmscharge { get; set; }
    }
}
