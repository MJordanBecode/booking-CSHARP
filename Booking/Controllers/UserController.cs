/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Solution.Models; // Ton modèle ApplicationUser
using System.Security.Claims;
using System.Threading.Tasks;

namespace Booking.Controllers
{
    [Authorize]
    [Route("api/[controller]")]    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber
            });
        }
    }
}*/