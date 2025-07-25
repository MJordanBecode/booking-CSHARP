namespace Solution.Models;

public class Offer
{
    public int Id { get; set; }
    
    public int IdUser { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Location { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
    
    public int BedNumber { get; set; }
    
    public int BathNumber { get; set; }
    
    public int NumberOfRooms { get; set; }
    
    public decimal Price { get; set; }
    
    public string Image { get; set; } = string.Empty;
    
    public ApplicationUser OfferUser { get; set; } = null!;
}