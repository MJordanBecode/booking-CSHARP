using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Solution.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    [Display(Name = "Prénom")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Nom")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Date de création")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    // PhoneNumber est déjà défini dans IdentityUser
    // Email est déjà défini dans IdentityUser
    // Password et ConfirmPassword sont gérés par Identity dans les ViewModels/DTOs

    [Display(Name = "Nom complet")]
    public string FullName => $"{FirstName} {LastName}";

    // Navigation vers vos entités personnalisées
    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}