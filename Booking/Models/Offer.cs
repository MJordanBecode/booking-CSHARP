using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Solution.Models;

namespace Booking.Models;

public class Offer
{
    public int Id { get; set; }
    
    public string? IdUser { get; set; } // Dans Identity, l'id est un string UUID exemple => 3f2504e0-4f89-11d3-9a0c-0305e82c3301
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Location { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
    
    public int BedNumber { get; set; }
    
    public int BathNumber { get; set; }
    
    public int NumberOfRooms { get; set; }
    
    public double Note { get; set; }
    
    public int Price { get; set; }
    
    [Column(TypeName = "nvarchar(max)")]
    public string Image { get; set; } = string.Empty;
    
    // Navigation property vers l'utilisateur
    [ValidateNever] // 👈 Ignore cette propriété lors de la validation du formulaire à retirer parès tous les test
    public ApplicationUser OfferUser { get; set; } = null!;

  
}