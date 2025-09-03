using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    public class User : IdentityUser<long>
    {
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string LastName { get; set; }
        [Compare("PasswordHash", ErrorMessage = "رمز عبور مطابقت ندارد")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public string ImagePath => string.IsNullOrEmpty(Image) ? "/images/users/UserDefaultIcon.png" : "/images/users/" + Image;
        public string RegisterCode { get; set; }
        public bool Registered { get; set; } = true;
       
        [ForeignKey("City")]
        public long? CityId { get; set; }
        public int? UserInsertedId { get; set; }

        

        //--------------------------------------------------------------
        public virtual ICollection<UserStation> UserStations { get; set; }
        public virtual ICollection<Correspondence> Correspondences { get; set; }
        public virtual ICollection<Notification> NotificationsRecive { get; set; }
        public virtual ICollection<Notification> NotificationsSend { get; set; }
        public virtual ICollection<UserAction> UserActions { get; set; }
        public virtual ICollection<UserLoginHistory> UserLoginHistorys { get; set; }
        public virtual ICollection<SmsSend> SmsSends { get; set; }
              public virtual City City { get; set; }


    }
}
