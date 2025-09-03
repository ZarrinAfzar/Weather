using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Enums
{
    public enum EnuFiles
    {
        [Display(Name = "نرم افزار")]
        SoftWare = 1,
        [Display(Name = "کاتالوگ")]
        Catalogues = 2,
        [Display(Name = "آرشیو")]
        Articles = 3
    }
}
