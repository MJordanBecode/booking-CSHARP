using System.Security.Claims;
using System.Threading.Tasks; // NÉCESSAIRE
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Booking.Models;
using Microsoft.AspNetCore.Identity;

namespace Booking.Controllers;

[Route("Profil")]
public class ProfilController : Controller
{
    private readonly ILogger<ProfilController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfilController(ILogger<ProfilController> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    // GET: /Profil
    [HttpGet("")]
    public async Task<IActionResult> SelfProfil()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return RedirectToAction("Login", "Account");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("Utilisateur non trouvé.");

        return View("Profil", user); // <- Reutilise la même vue
    }

    // GET: /Profil/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> ProfilById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToAction("Login", "Account");

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound("Utilisateur non trouvé.");

        return View("Profil", user); // <- Même vue aussi
    }
}
