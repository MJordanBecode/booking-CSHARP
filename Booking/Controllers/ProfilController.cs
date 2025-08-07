using System.Security.Claims;
using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// si ton DbContext est ici
// <-- à ajouter

namespace Booking.Controllers;

public class ProfilController : Controller
{
    private readonly ILogger<ProfilController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public ProfilController(ILogger<ProfilController> logger,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    [Route("Profil/SelfProfil")]
    public async Task<IActionResult> SelfProfil(int page = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return RedirectToAction("Login", "Account");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("Utilisateur non trouvé.");

        int pageSize = 5;

        var query = _context.Offers.Where(o => o.IdUser == userId);

        int totalOffers = await query.CountAsync();

        var offersPaged = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new ProfileViewModel
        {
            User = user,
            Offers = offersPaged,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalOffers / (double)pageSize)
        };

        return View("Profil", viewModel);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ProfilById(string id, int page = 1)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToAction("Login", "Account");

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound("Utilisateur non trouvé.");

        int pageSize = 5;

        var query = _context.Offers.Where(o => o.IdUser == id);

        int totalOffers = await query.CountAsync();

        var offersPaged = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new ProfileViewModel
        {
            User = user,
            Offers = offersPaged,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalOffers / (double)pageSize)
        };

        return View("Profil", viewModel);
    }
    

}