// using Microsoft.EntityFrameworkCore;
// using Booking.Data;
// using Solution.Models;
// using Solution.Service;
// using Solution.Extensions;
//
//
// namespace Booking.Service;
//
// public class OfferService : IOfferService
// {
//     private readonly ApplicationDbContext _context;
//
//     public OfferService(ApplicationDbContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<IEnumerable<Offer>> GetAllOffersAsync()
//     {
//         return await _context.Offers.ToListAsync();
//     }
//
//     public async Task<Offer?> GetOfferBy IdAsync(int id)
//     {
//         return await _context.Offers.FirstOrDefaultAsync(o => o.Id == id);
//     }
//
//     public async Task<Offer> GetOfferWithRelationsAsync(int id)
//     {
//         return await _context.Offers
//             .Include(o => o.OfferUser)
//             .FirstOrDefaultAsync(o => o.Id == id);
//     }
//
//     public async Task<Offer> CreateOfferAsync(Offer offer)
//     {
//         _context.Offers.Add(offer);
//         await _context.SaveChangesAsync();
//         return offer;
//     }
//
//     public async Task<Offer?> UpdateOfferAsync(Offer offer)
//     {
//         var existingOffer = await _context.Offers.FindAsync(offer.Id);
//         if (existingOffer == null)
//         {
//             return null;
//         }
//
//         // Mise à jour des propriétés
//         existingOffer.Title = offer.Title;
//         existingOffer.Description = offer.Description;
//         existingOffer.Location = offer.Location;
//         existingOffer.Type = offer.Type;
//         existingOffer.BedNumber = offer.BedNumber;
//         existingOffer.BathNumber = offer.BathNumber;
//         existingOffer.NumberOfRooms = offer.NumberOfRooms;
//         existingOffer.Price = offer.Price;
//         existingOffer.Image = offer.Image;
//
//
//         try
//         {
//             await _context.SaveChangesAsync();
//             return existingOffer;
//         }
//         catch (DbUpdateConcurrencyException)
//         {
//             throw;
//         }
//     }
//         public async Task<bool> DeleteOfferAsync(int id, int userId)
//         {
//             try
//             {
//                 var offer = await _context.Offers.FindAsync(id);
//                 if (offer == null || offer.IdUser != userId)
//                 {
//                     return false; // Offre non trouvée ou pas le bon propriétaire
//                 }
//
//                 _context.Offers.Remove(offer);
//                 var result = await _context.SaveChangesAsync();
//         
//                 return result > 0;
//             }
//             catch (Exception)
//             {
//                 return false;
//             }
//         }
//
//         public async Task<bool> OfferExistsAsync(int id)
//         {
//             return await _context.Offers.AnyAsync(o => o.Id == id);
//         }
//     }
