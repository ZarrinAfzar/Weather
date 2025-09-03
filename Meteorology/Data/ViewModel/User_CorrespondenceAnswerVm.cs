using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Data.ViewModel
{
    public class User_CorrespondenceAnswerVm
    {
        public Correspondence Correspondence { get; set; }
        public List<Correspondence> CorrespondenceList { get; set; }

    }
}
