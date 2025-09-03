using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather.Models
{
    public class LastData : BaseModel
    {

        public int Port { get; set; }
        public int Bytes { get; set; }
        public int Type { get; set; }
        public string SerialNO { get; set; }
        public string StationName { get; set; }
        public DateTime DatetimeEn { get; set; }
        public string DatetimeFa { get; set; }
        public string DataPacket { get; set; }
        //------------------------------------------------------------


    }
}
