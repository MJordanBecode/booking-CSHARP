using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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

        // Accessible à tous (y compris les guests non connectés)
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var offers = await _offerService.GetAllOffersAsync();
            return View(offers);
        }

        // Accessible à tous (y compris les guests non connectés)
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var offer = await _offerService.GetOfferWithRelationsAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        // Seuls les Hosts et Admins peuvent créer des offres
        [Authorize(Policy = "HostOrAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HostOrAdmin")]
        public async Task<IActionResult> Create([Bind("Title,Description,Location,Type,BedNumber,BathNumber,NumberOfRooms,Price,Image")] Offer offer)
        {
            // Récupérer l'ID de l'utilisateur connecté
            offer.IdUser = GetCurrentUserId();

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

            return View(offer);
        }

        // Seul le propriétaire de l'offre ou l'admin peut modifier
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var offer = await _offerService.GetOfferByIdAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est le propriétaire ou un admin
            if (!await CanUserModifyOffer(offer))
            {
                return Forbid("Vous n'êtes pas autorisé à modifier cette offre.");
            }

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,Type,BedNumber,BathNumber,NumberOfRooms,Price,Image")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            var existingOffer = await _offerService.GetOfferByIdAsync(id);
            if (existingOffer == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est le propriétaire ou un admin
            if (!await CanUserModifyOffer(existingOffer))
            {
                return Forbid("Vous n'êtes pas autorisé à modifier cette offre.");
            }

            // Conserver l'ID du propriétaire original
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

            return View(offer);
        }

        // Seul le propriétaire de l'offre ou l'admin peut supprimer
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var offer = await _offerService.GetOfferByIdAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            // Vérifier si l'utilisateur est le propriétaire ou un admin
            if (!await CanUserModifyOffer(offer))
            {
                return Forbid("Vous n'êtes pas autorisé à supprimer cette offre.");
            }

            return View(offer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var offer = await _offerService.GetOfferByIdAsync(id);
                if (offer == null)
                {
                    return NotFound();
                }

                // Vérifier si l'utilisateur est le propriétaire ou un admin
                if (!await CanUserModifyOffer(offer))
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
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Une erreur inattendue s'est produite.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Méthodes utilitaires privées
        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        private async Task<bool> OfferExists(int id)
        {
            return await _offerService.OfferExistsAsync(id);
        }

        private async Task<bool> CanUserModifyOffer(Offer offer)
        {
            var currentUserId = GetCurrentUserId();
            
            // L'admin peut tout modifier
            if (User.IsInRole("Admin"))
            {
                return true;
            }
            
            // Le propriétaire peut modifier ses propres offres
            return offer.IdUser == currentUserId;
        }

        // Action réservée aux admins - Gestion de toutes les offres
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminManage()
        {
            var allOffers = await _offerService.GetAllOffersAsync();
            return View("AdminManage", allOffers);
        }

        // Action pour voir ses propres offres (Host)
        [Authorize(Policy = "HostOrAdmin")]
        public async Task<IActionResult> MyOffers()
        {
            var currentUserId = GetCurrentUserId();
            var offers = await _offerService.GetAllOffersAsync();
            var userOffers = offers.Where(o => o.IdUser == currentUserId).ToList();
            
            return View(userOffers);
        }
    }
}