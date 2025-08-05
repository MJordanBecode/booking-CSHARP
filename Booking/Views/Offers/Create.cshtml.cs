using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Solution.Models;
// <-- pour IFormFile
// <-- pour IWebHostEnvironment

namespace Booking.Views.Offers  
{
    [Authorize(Roles = "Host")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;  // <-- injecter l'environnement web

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Offer Offer { get; set; } = null!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }  // <-- propriété pour recevoir le fichier uploadé

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Offer.IdUser = userId;

            if (ImageFile != null)
            {
                // Générer un nom unique pour éviter collisions
                var fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                var extension = Path.GetExtension(ImageFile.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                // Chemin complet vers wwwroot/images
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "offres");
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Stocker le chemin relatif dans la base
                Offer.Image = Path.Combine("images", uniqueFileName).Replace("\\", "/");
            }

            _context.Offers.Add(Offer);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Home/Index");
        }
    }
}
