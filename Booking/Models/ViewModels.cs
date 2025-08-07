using Booking.Models;
namespace Booking.Models
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; } = null!;
        public List<Offer> Offers { get; set; } = new List<Offer>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}