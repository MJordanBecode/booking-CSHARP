using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Solution.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(55)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(55)]
    public string LastName { get; set; }
    
    [Required]
    [StringLength(30)]
    public string PhoneNumber { get; set; }
    

    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    // Navigation vers vos entités personnalisées
    public virtual ICollection<Offer> Offers { get; set; }
}