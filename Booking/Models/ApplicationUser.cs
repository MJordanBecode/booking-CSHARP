using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get; set; }
        
        // Propriété calculée pour afficher le nom complet
        public string FullName => $"{FirstName} {LastName}";
    }
}