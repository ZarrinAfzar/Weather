using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Weather.Models;

namespace Meteorology.Models
{
    public class UserProject
    {
        [Key]
        public int Id { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;

        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
    }
}
