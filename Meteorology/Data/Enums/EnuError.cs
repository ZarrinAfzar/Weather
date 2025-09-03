using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Enums
{
    public enum EnuError
    {
        [Display(Name = "باتری معیوب است")]
        Battery = 1,
        [Display(Name = "سولار کثیف است")]
        Solar = 2,
        [Display(Name = "عدم ارسال داده")]
        SendData = 3,
        [Display(Name = "سنسور دما و رطوبت معیوب است")]
        TempHumEr = 4,
        [Display(Name = "سنسور خیسی برگ معیوب است")]
        LeafEr = 5,
        [Display(Name = " سیم کارت شارژ ندارد")]
        Charge = 6,
        [Display(Name = " سیم کارت نت ندارد")]
        Gprs = 7,
        [Display(Name = "تنظیمات ایستگاه پاک شده است")]
        Setting = 8,
        [Display(Name = "دیتالاگر دچار مشکل سخت افزاری شده است ")]
        LoggerDameged = 9,
        [Display(Name = "مودم دچار مشکل سخت افزاری شده است ")]
        ModemDameged = 10,
        [Display(Name = "فاصله زمانی ارسال داده درست تنظیم نشده است ")]
        Interval = 11,
        [Display(Name = "کارفرما ایستگاه را خاموش گذاشته است ")]
        UserOff = 12,
        [Display(Name = "تاریخ و ساعت ایستگاه تنظیم نیست")]
        Clock = 13,
    }
}
