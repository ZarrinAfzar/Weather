using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Weather.Models
{
    public class ReceivedSMS : BaseModel
    {
        
        public string From { get; set; }


        public string Message { get; set; }


        public DateTime? ReceivedTime { get; set; }
        public long? StationId { get; set; }
        public string StationName { get; set; }
        public string DatePrs { get; set; }
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDatePrs { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndDatePrs { get; set; }
        public double? Value { get; set; }
        public int? Process { get; set; }
        public bool Accepted { get; set; }
        public string Description { get; set; }


    }
}
