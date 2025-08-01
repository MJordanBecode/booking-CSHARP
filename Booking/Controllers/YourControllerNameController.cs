using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Booking.Service;
using Solution.Models;
using Solution.Service;

namespace Booking.Controllers
{
    public class OffersController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment;

        public OffersController(IOfferService offerService, IUserService userService, IWebHostEnvironment environment)
        {
            _offerService = offerService;
            _userService = userService;
            _environment = environment;
        }

    
        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var offers = await _offerService.GetAllOffersAsync();
            return View("~/Views/Home/Index.cshtml", offers);  // ✅ PAS de transformation en CardViewModel
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var offer = await _offerService.GetOfferWithRelationsAsync(id);
            if (offer == null)
                return NotFound();

            return View(offer);
        }

        [Authorize(Policy = "HostOrAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HostOrAdmin")]
        public async Task<IActionResult> Create(Offer offer, IFormFile ImageFile)
        {
            offer.IdUser = GetCurrentUserId(); // récupère l'ID utilisateur connecté

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "offres");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    var extension = Path.GetExtension(ImageFile.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    offer.Image = Path.Combine("images", "offres", uniqueFileName).Replace("\\", "/");
                }

                await _offerService.CreateOfferAsync(offer);
                return RedirectToAction("Index", "Home");
            }

            return View(offer);
        }


        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var offer = await _offerService.GetOfferByIdAsync(id);
            if (offer == null)
                return NotFound();

            if (!await CanUserModifyOffer(offer))
                return Forbid("Vous n'êtes pas autorisé à modifier cette offre.");

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,Type,BedNumber,BathNumber,NumberOfRooms,Price,Image")] Offer offer)
        {
            if (id != offer.Id)
                return NotFound();

            var existingOffer = await _offerService.GetOfferByIdAsync(id);
            if (existingOffer == null)
                return NotFound();

            if (!await CanUserModifyOffer(existingOffer))
                return Forbid("Vous n'êtes pas autorisé à modifier cette offre.");

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
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(offer);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var offer = await _offerService.GetOfferByIdAsync(id);
            if (offer == null)
                return NotFound();

            if (!await CanUserModifyOffer(offer))
                return Forbid("Vous n'êtes pas autorisé à supprimer cette offre.");

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
                    return NotFound();

                if (!await CanUserModifyOffer(offer))
                    return Forbid("Vous n'êtes pas autorisé à supprimer cette offre.");

                bool deleted = await _offerService.DeleteOfferAsync(id, GetCurrentUserId());

                if (deleted)
                    TempData["SuccessMessage"] = "Offre supprimée avec succès !";
                else
                    TempData["ErrorMessage"] = "Erreur lors de la suppression de l'offre.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Une erreur inattendue s'est produite.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Json(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserRoles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var roles = await _userService.GetUserRolesAsync(userId);
            return Json(new { roles });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminManage()
        {
            var allOffers = await _offerService.GetAllOffersAsync();
            return View("AdminManage", allOffers);
        }

        [Authorize(Policy = "HostOrAdmin")]
        public async Task<IActionResult> MyOffers()
        {
            var currentUserId = GetCurrentUserId();
            var offers = await _offerService.GetAllOffersAsync();
            var userOffers = offers.Where(o => o.IdUser == currentUserId).ToList();

            return View(userOffers);
        }

        // Méthodes utilitaires
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

            if (User.IsInRole("Admin"))
                return true;

            return offer.IdUser == currentUserId;
        }
    }
}
