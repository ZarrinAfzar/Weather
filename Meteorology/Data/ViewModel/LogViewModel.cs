using Weather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.ViewModel
{
    public class LogViewModel
    {
        public List<UserLoginHistory> UserLoginHistorys { get; set; }
        public List<UserAction> UserActions { get; set; }
    }
}
 