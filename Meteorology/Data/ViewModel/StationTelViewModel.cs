using Weather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class StationTelViewModel
    {
        public StationTel StationTel { get; set; }
        public IEnumerable<StationTel> StationTels { get; set; }
    }
}
