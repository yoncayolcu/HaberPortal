using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HaberPortal.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="Lütfen adınızı giriniz.")]
        [StringLength(100)]
        [Display(Name = "Tam isim")]
        public string FullName { get; set; }
    }

}
