using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;

namespace Weather.Models
{
    /// <summary>
    /// آپلود فایل
    /// </summary>
    public class Files : BaseModel
    {

        public DateTime InsertDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        /// <summary>
        /// عنوان فایل
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// توضیحات فایل
        /// </summary>
        public string Desctription { get; set; }
        /// <summary>
        /// آدرس فایل
        /// </summary>
        public string File { get; set; }
        /// <summary>
        /// نوع فایل
        /// </summary>
        public EnuFiles Type { get; set; }

    }
}
