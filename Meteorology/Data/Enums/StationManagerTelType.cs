using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Enums
{ 
    /// <summary>
    /// حقیقی حقوقی
    /// </summary>
    public enum ManagerTelType
    {
        [Display(Name = "مدیریت")]
        Manager = 0,
        [Display(Name ="مدیر متصدیان")]
        OPManager = 1,
       
    } 
}
