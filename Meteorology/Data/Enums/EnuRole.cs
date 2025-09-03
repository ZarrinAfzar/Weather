using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Enums
{
    public enum EnuRole
    {
        [Display(Name = "مدیر ارشد")]
        AdminiStratore = 1,
        [Display(Name = "پروژه ها")]
        Project = 2,
        #region Project
        [Display(Name = "ثبت پروژه")]
        ProjectNew = 3,
        [Display(Name = "ویرایش پروژه")]
        ProjectEdit = 4,
        [Display(Name = "حذف پروژه")]
        ProjectDelete = 5,
        [Display(Name = " شارژ پیامک پروژه")]
        ProjectSmsRequest = 6,
        [Display(Name = " جزئیات پروژه")]
        ProjectView = 7,
        #endregion
        [Display(Name = "ایستگاه ها")]
        Station = 8,
        #region Station
        [Display(Name = "ثبت ایستگاه")]
        StationNew = 9,
        [Display(Name = "ویرایش ایستگاه")]
        StationEdit = 10,
        [Display(Name = "حذف ایستگاه")]
        StationDelete = 11,
        [Display(Name = "جزئیات ایستگاه")]
        StationView = 12,
        [Display(Name = "آپلود فایل ایستگاه")]
        StationUpload = 13,
        [Display(Name = "تلفن های ایستگاه")]
        StationTel = 14,
        [Display(Name = "جزئیات پروژه ایستگاه")]
        StationProject = 15,
        [Display(Name = "پیامک ایستگاه")]
        StationSms = 16,
        [Display(Name = "سنسورها")]
        SensorSetting = 17,
        [Display(Name = "ویرایش تنظیمات سنسورها")]
        EditSensorSetting = 18,
        #endregion
        [Display(Name = "تنظیمات آلارم")]
        Alarms = 19,
        #region Alarms
        [Display(Name = "ثبت آلارم")]
        NewAlarms = 21,
        [Display(Name = "ویرایش  آلارم")]
        EditAlarms = 22,
        [Display(Name = "حذف آلارم")]
        DeleteAlarms = 23,
        [Display(Name = "تلفن های آلارم")]
        AlarmsTel = 24,
        [Display(Name = "ایستگاه آلارم")]
        AlarmsStation = 25,
        [Display(Name = "آلارم سنسورها")]
        SensorAlarmDetail = 26,
        [Display(Name = "ویرایش آلارم سنسورها")]
        EditSensorAlarmDetail = 27,
        [Display(Name = "حذف آلارم سنسورها")]
        DeleteSensorAlarmDetail = 28,
        [Display(Name = "آلارم آفات")]
        PestAlarmDetail = 29,
        [Display(Name = "ویرایش آلارم آفات")]
        EditPestAlarmDetail = 30,
        #endregion
        [Display(Name = "گزارش آلارم های ثبت شده")]
        AlarmReport = 31,
        [Display(Name = "ثبت جزئیات آلارم سنسور")]
        NewSensorAlarmDetail = 32,
        [Display(Name = "گزارش شارژ پیامک های انجام شده")]
        SmsCharge = 33,
        [Display(Name = "گزارش سنسورها")]
        SensorReport = 34,
        [Display(Name = "دانلود ها")]
        Download = 35,
        [Display(Name = "آپلود ها")]
        Upload = 36,
        [Display(Name = "انتقادات و پیشنهادات")]
        Correspondence = 37,
        [Display(Name = "پیام ها")]
        Notification = 38,
        [Display(Name = "ارسال پیام")]
        SendNotification = 39,
        [Display(Name = "کاربران")]
        User = 40,
        #region User
        [Display(Name = "ثبت کاربر")]
        UserNew = 41,
        [Display(Name = "ویرایش کاربر")]
        UserEdit = 42,
        [Display(Name = "حذف کاربر")]
        UserDelete = 43,
        [Display(Name = "دسترسی های کاربر")]
        UserAccess = 44,
        [Display(Name = "پیام های کاربر")]
        UserNotification = 46,
        [Display(Name = "ایستگاه های کاربر")]
        UserStation = 47,
        [Display(Name = "فعالیت های کاربر")]
        UserSystemAction = 48,
        #endregion
        [Display(Name = "اطلاعات پایه")]
        BaseInfo = 49,
        #region BaseInfo
        [Display(Name = "استان ها")]
        BaseInfoState = 50,
        [Display(Name = "شهرستانها")]
        BaseInfoCities = 51,
        [Display(Name = "دیتالاگر")]
        BaseInfoDataLoger = 52,
        [Display(Name = "نوع مودم")]
        BaseInfoModemType = 53,
        [Display(Name = "نوع ایستگاه")]
        BaseInfoStationType = 54,
        [Display(Name = "نام استاندارد سنسورها")]
        SensorType = 55,
        //[Display(Name = "پارامترهای پیش بینی")]
        //ForecastsAlarmParameter = 56,
        #endregion

        [Display(Name = "مدیریت انتقادات و پیشنهادات")]
        AdminCorrespondence = 57,
        [Display(Name = "نمودار")]
        Chart = 58,
        [Display(Name = "نقشه")]
        Map = 59,
        [Display(Name = "نمودار درجه روز رشد")]
        DDChart = 60,
        [Display(Name = "نمودار گلباد")]
        WindChart = 61,
        [Display(Name = "گزارش نیاز سرمایی")]
        GCDReport = 62,
        [Display(Name = "گزارش گپ داده")]
        GapReport = 63,
        [Display(Name = "گزارش خیسی برگ")]
        LeafWetnessReport = 64,
        [Display(Name = "گزارش نیاز گرمایی")]
        GDDReport = 65,
        [Display(Name = "گزارش خطای داده")]
        DateErorrRport = 66,
        [Display(Name = "مدیر سازمان")]
        AdminUser = 67,
        [Display(Name = "پیامک های دریافتی")]
        ReceivedSMS = 68,
        [Display(Name = "پیامک های ارسالی")]
        SendSMS = 69,
        [Display(Name = "گزارش وضعیت ایستگاه ها")]
        StationStatus = 70,
             [Display(Name = "دفترچه تلفن مدیران")]
        ManagerTel = 71,
        [Display(Name = "گزارش مجموع باران")]
        RainReport = 72,
        [Display(Name = "گزارش مجموع تبخیر")]
        EvaporationReport = 73,
          [Display(Name = "ارسال پیامک")]
        SMSPanel = 74  
    }

    public enum EnuRoleBase
    {
        [Display(Name = "دانلود ها")]
        Download = 35,
        [Display(Name = "انتقادات و پیشنهادات")]
        Correspondence = 37,
        [Display(Name = "ایستگاه ها")]
        Station = 8,
        [Display(Name = "پیام ها")]
        Notification = 38,
        [Display(Name = "کاربران ایستگاه")]
        StationUser = 39
       
    }
}
