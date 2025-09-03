using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Enums
{
    public enum EnuAlarm
    {
        [Display(Name = "آلارم سنسور ها")]
        Sensors = 1,
       
        [Display(Name = "آلارم آفات")]
        Pest = 3,
    }
}
