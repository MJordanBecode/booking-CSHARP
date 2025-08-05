using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Booking.Data;
using Booking.Models;
using Booking.Service;
using Solution.Models;

namespace Booking.Service;

public interface IOfferService
{
    Task<IEnumerable<Offer>> GetAllOffersAsync();
    Task<Offer> GetOfferByIdAsync(int id);

    Task<Offer?> GetOfferWithRelationsAsync(int id);

    Task<Offer> CreateOfferAsync(Offer offer);

    Task<Offer?> UpdateOfferAsync(Offer offer);

    Task<bool> DeleteOfferAsync(int id, string userId);
    Task<bool> OfferExistsAsync(int id);
}