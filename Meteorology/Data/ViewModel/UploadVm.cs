using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Data.ViewModel
{
    public class UploadVm
    {
        public Files Files { get; set; }
        public List<Files> FilesList { get; set; }
    }
}
