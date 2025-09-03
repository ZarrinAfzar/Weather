using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    public class Role : IdentityRole<long>
    {
        public Role()
        {
        }
        public Role(string name)
            : this()
        {
            Name = name;
        }

        public Role(string name, string description)
            : this(name)
        {
            Description = description;
        }

        public string Description { get; set; }

        //public virtual ICollection<UserRole> Users { get; set; }

        //public virtual ICollection<RoleClaim> Claims { get; set; }

    }
}
