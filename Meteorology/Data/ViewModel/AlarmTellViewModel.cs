using Weather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class AlarmTellViewModel 
    {
        public int AlarmId { get; set; }
        
        public List<AlarmTell> AlarmTells { get; set; }
        public List<StationTel> StationTels { get; set; }
    }
}
