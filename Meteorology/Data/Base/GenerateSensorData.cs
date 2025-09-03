using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Base
{
    public static class GenerateSensorData
    {
        public static bool dataState(this string val) => val.Contains("false") ? false : true;
        public static double dataGetValue(this string val)
        {
            try
            {
                return string.IsNullOrEmpty(val) ? 0 :
                     val.Contains("false") ?
                        Convert.ToDouble(val.Substring(0, val.IndexOf("f")).Trim(), CultureInfo.InvariantCulture) :
                        Convert.ToDouble(val.Substring(0, val.IndexOf("t")).Trim(), CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static int dataGetDigit(this string val) => string.IsNullOrEmpty(val) ? 0 : Convert.ToInt32(val.Substring(val.IndexOf("@") + 1));
    }
}
