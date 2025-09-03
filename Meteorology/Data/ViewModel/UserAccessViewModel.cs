using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class UserAccessViewModel
    { 
        public long UserId { get; set; }
        public string Description { get; set; } 
        public string Value { get; set; }
        public bool State { get; set; }
    }
}
