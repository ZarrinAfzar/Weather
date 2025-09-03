using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class ProjectVm 
    {
        public int Id { get; set; }
        public string InsertDate { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public int SmsCount { get; set; }
        public int Stations { get; set; } 

    }
}
