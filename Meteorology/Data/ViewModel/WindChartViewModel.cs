using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class WindChartViewModel
    {
        public string Direction { get; set; }
        public List<object> Speeds { get; set; } 
    }
}
