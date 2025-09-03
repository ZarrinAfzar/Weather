using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Weather.Data.ViewModel
{
    public class UserViewModel
    { 
        public User User { get; set; }
               
        public List<string> StationList { get; set; }
        public List<string> ProjectList { get; set; }
        public List<UserAction> UserActionList { get; set; }
     
    } 
}
