using System;
using System.ComponentModel.DataAnnotations;

namespace Weather.Models
{
    public class VersionHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;

        public string Notes { get; set; }
    }
}
