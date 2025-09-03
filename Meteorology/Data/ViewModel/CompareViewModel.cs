using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class CompareViewModel
    {
        public DataTable AllTempreture { get; set; }
        public DataTable AllHumedity { get; set; }
        public DataTable AllRain { get; set; }

        public DataTable CountTempreture { get; set; }
        public DataTable CountHumedity { get; set; }
        public DataTable CountRain { get; set; }

    }
}
