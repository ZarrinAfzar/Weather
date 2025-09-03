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
    public enum ReceivedSMSType
    {
        [Display(Name = "حقیقی(فیزیکی)")]
        Regular = 1,
        [Display(Name = "مجازی(محاسباتی)")]
        Virtual = 2,
    } 
}
