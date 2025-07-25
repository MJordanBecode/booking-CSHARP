using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Booking.Service;
using Solution.Service;
using Solution.Extensions;
using Solution.Models;

namespace Solution.Controllers
{
    public class YourControllerNameController : Controller
    {
        private readonly IOfferService _offerService;

        public YourControllerNameController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IActionResult> Index()
        {
            var item = await _offerService.GetAllOffersAsync();
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _offerService.GetOfferByIdAsync(id);
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
        
        private int GetCurrentUserId()
        {
            return User.GetUserId();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Location,Type,BedNumber,BathNumber,NumberOfRooms,Price,Image")] Offer offer)
        {
            // Assigner l'utilisateur connecté (sécurité)
            offer.IdUser = GetCurrentUserId(); 
    
            if (ModelState.IsValid)
            {
                await _offerService.CreateOfferAsync(offer);
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,Type,BedNumber,BathNumber,NumberOfRooms,Price,Image")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            // Vérifier que l'utilisateur connecté est le propriétaire de l'offre
            try
            {
                var existingOffer = await _offerService.GetOfferByIdAsync(id);
                if (existingOffer == null)
                {
                    return NotFound();
                }

                // Sécurité : vérifier que l'utilisateur connecté est le propriétaire
                if (existingOffer.IdUser != GetCurrentUserId())
                {
                    return Forbid();
                }

                // Conserver l'IdUser original (sécurité)
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

                // Vérifier que l'utilisateur connecté est le propriétaire
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

// POST: Offer/Delete/5 - Suppression confirmée
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Double vérification de sécurité
                var offer = await _offerService.GetOfferByIdAsync(id);
                if (offer == null)
                {
                    return NotFound();
                }

                // Vérifier que l'utilisateur connecté est le propriétaire
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

// Méthode helper mise à jour pour utiliser le nouveau service
        private async Task<bool> OfferExists(int id)
        {
            return await _offerService.OfferExistsAsync(id);
        }

        
        
    }
}