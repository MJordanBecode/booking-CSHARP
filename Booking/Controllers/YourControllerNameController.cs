using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Booking.Service;
using Solution.Models;

namespace Booking.Controllers
{
    public class OffersController : Controller
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IActionResult> Index()
        {
            var item = await _offerService.GetAllOffersAsync();
            return View(item);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _offerService.GetOfferWithRelationsAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Location,Type,BedNumber,BathNumber,NumberOfRooms,Price,Image")] Offer offer)
        {
            // ⛔ Temporairement, tu peux forcer un Id fictif si l'authentification n'est pas encore active
            offer.IdUser = "test-user"; // ← À remplacer plus tard par GetCurrentUserId()

            if (ModelState.IsValid)
            {
                Console.WriteLine("Form is valid — inserting offer");
                await _offerService.CreateOfferAsync(offer);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("Form is invalid");
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine("Validation error: " + error.ErrorMessage);
                    }
                }
            }

            // ⬅ Retourner la vue avec l’objet pour réafficher les erreurs côté Razor
            return View(offer);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var item = await _offerService.GetOfferByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            if (item.IdUser != GetCurrentUserId())
            {
                return Forbid();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,Type,BedNumber,BathNumber,NumberOfRooms,Price,Image")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            try
            {
                var existingOffer = await _offerService.GetOfferByIdAsync(id);
                if (existingOffer == null)
                {
                    return NotFound();
                }

                if (existingOffer.IdUser != GetCurrentUserId())
                {
                    return Forbid();
                }

                offer.IdUser = existingOffer.IdUser;

                if (ModelState.IsValid)
                {
                    try
                    {
                        await _offerService.UpdateOfferAsync(offer);
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await OfferExists(offer.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(offer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var offer = await _offerService.GetOfferByIdAsync(id);
                if (offer == null)
                {
                    return NotFound();
                }

                if (offer.IdUser != GetCurrentUserId())
                {
                    return Forbid("Vous n'êtes pas autorisé à supprimer cette offre.");
                }

                return View(offer);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var offer = await _offerService.GetOfferByIdAsync(id);
                if (offer == null)
                {
                    return NotFound();
                }

                if (offer.IdUser != GetCurrentUserId())
                {
                    return Forbid("Vous n'êtes pas autorisé à supprimer cette offre.");
                }

                bool deleted = await _offerService.DeleteOfferAsync(id, GetCurrentUserId());

                if (deleted)
                {
                    TempData["SuccessMessage"] = "Offre supprimée avec succès !";
                }
                else
                {
                    TempData["ErrorMessage"] = "Erreur lors de la suppression de l'offre.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Une erreur inattendue s'est produite.";
                return RedirectToAction(nameof(Index));
            }
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private async Task<bool> OfferExists(int id)
        {
            return await _offerService.OfferExistsAsync(id);
        }
        
        
    }
    
}
