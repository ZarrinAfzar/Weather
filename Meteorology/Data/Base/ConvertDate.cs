using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Weather.Data.Base
{
    public static class ConvertDate  
    {
        private static CultureInfo _Culture;
        public static CultureInfo GetPersianCulture()
        {
            if (_Culture == null)
            {
                _Culture = new CultureInfo("fa-IR");
                DateTimeFormatInfo formatInfo = _Culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند",
                    ""
                };
                formatInfo.AbbreviatedMonthNames =
                    formatInfo.MonthNames =
                        formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd";
                formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
                System.Globalization.Calendar cal = new PersianCalendar();

                FieldInfo fieldInfo = _Culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                    fieldInfo.SetValue(_Culture, cal);

                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null)
                    info.SetValue(formatInfo, cal);

                _Culture.NumberFormat.NumberDecimalSeparator = "/";
                _Culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _Culture.NumberFormat.NumberNegativePattern = 0;
            }
            return _Culture;
        }



        /// <summary>
        /// "yyyy/MM/dd" : 1391/12/13
        /// "dddd, dd MMMM,yyyy" :یکشنبه, 13 اسفند,1391
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToPeString(this DateTime date, string format = "yyyy/MM/dd HH:mm:ss")
        {
            return date.ToString(format, GetPersianCulture());
        }
        public static string ToPeString(this DateTime? date, string format = "yyyy/MM/dd HH:mm:ss")
        {
            return date?.ToString(format, GetPersianCulture());
        }
        public static DateTime ToGreDateTime(this string Pedate, bool shortDate = false)
        {
            string pedateConverted = Regex.Replace(Pedate, "[۰-۹]", x => ((char)(x.Value[0] - '۰' + '0')).ToString());
            int year = Convert.ToInt32(pedateConverted.Substring(0, 4));
            int month = Convert.ToInt32(pedateConverted.Substring(5, 2));
            int day = Convert.ToInt32(pedateConverted.Substring(8, 2));

            if (!shortDate)
            {
                int hour = DateTime.Now.Hour;
                int minute = DateTime.Now.Minute;
                int second = DateTime.Now.Second;
                if (pedateConverted.Length > 10)
                {
                    hour = Convert.ToInt32(pedateConverted.Substring(11, 2));
                    minute = Convert.ToInt32(pedateConverted.Substring(14, 2));
                    second = Convert.ToInt32(pedateConverted.Substring(17, 2));
                }
                PersianCalendar pc = new PersianCalendar();
                return pc.ToDateTime(year, month, day, hour, minute, second, 0);
            }
            else
            {
                PersianCalendar pc = new PersianCalendar();
                return pc.ToDateTime(year, month, day, 0, 0, 0, 0).Date;
            }

        }

    }
}
