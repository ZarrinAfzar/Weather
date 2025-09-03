using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    /// <summary>
    /// فایل های ایستگاه
    /// </summary>
    public class StationFile : BaseModel
    { 
        /// <summary>
        /// کد ایستگاه
        /// </summary>
        [ForeignKey("Station")]
        public long StationId { get; set; }
        /// <summary>
        /// عنوان فایل
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// توضیحات فایل
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        public string FileAddress { get; set; }


        //-------------------------------------------------------
        public virtual Station Station { get; set; }
    }
}
