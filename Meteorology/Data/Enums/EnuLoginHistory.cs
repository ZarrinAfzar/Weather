using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Enums
{
    public enum EnuLoginHistory
    {
        [Display(Name = "ورود به سیستم")]
        Login = 1,
        [Display(Name = "خروج از سیستم")]
        Logout = 2, 
    }
}
