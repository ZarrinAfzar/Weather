using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Data.ViewModel
{
    public class SmsSetToStationVm
    {
        public int ProjectId { get; set; }
        public int StationId { get; set; }
        public int SmsCount { get; set; }

        public List<Station> Stations { get; set; }
        public int HasSms { get; set; }
        
    }
}
