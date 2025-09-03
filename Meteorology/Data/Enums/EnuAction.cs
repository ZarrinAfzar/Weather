using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Enums
{
    public enum EnuAction
    {
        [Display(Name = "ثبت در")]
        Create = 1,
        [Display(Name = "ویرایش")]
        Update = 2,
        [Display(Name = "حذف از")]
        Delete = 3,
    }
}
