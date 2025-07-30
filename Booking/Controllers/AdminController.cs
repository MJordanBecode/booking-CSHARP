using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Solution.Models;
using Solution.Service;

namespace Booking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        // Dashboard admin
        public async Task<IActionResult> Index()
        {
            var allUsers = _userManager.Users.ToList();
            
            var usersWithRoles = new List<(ApplicationUser User, IList<string> Roles)>();
            
            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add((user, roles));
            }
            
            return View(usersWithRoles);
        }

        // Promouvoir un utilisateur en Host
        [HttpPost]
        public async Task<IActionResult> PromoteToHost(string userId)
        {
            var success = await _userService.PromoteToHostAsync(userId);
            
            if (success)
            {
                TempData["SuccessMessage"] = "Utilisateur promu Host avec succès.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erreur lors de la promotion.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // Rétrograder un Host en Guest
        [HttpPost]
        public async Task<IActionResult> DemoteFromHost(string userId)
        {
            var success = await _userService.DemoteFromHostAsync(userId);
            
            if (success)
            {
                TempData["SuccessMessage"] = "Host rétrogradé en Guest avec succès.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erreur lors de la rétrogradation.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // Voir tous les utilisateurs par rôle
        public async Task<IActionResult> UsersByRole(string role)
        {
            if (string.IsNullOrEmpty(role))
                return BadRequest();

            var users = await _userService.GetUsersInRoleAsync(role);
            ViewBag.RoleName = role;
            
            return View(users);
        }

        // Gestion des rôles d'un utilisateur spécifique
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var userRoles = await _userService.GetUserRolesAsync(userId);
            var allRoles = new[] { "Admin", "Host", "Guest" };

            var model = new
            {
                User = user,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            return View(model);
        }

        // Assigner un rôle à un utilisateur
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var success = await _userService.AssignRoleToUserAsync(userId, roleName);
            
            if (success)
            {
                TempData["SuccessMessage"] = $"Rôle {roleName} assigné avec succès.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erreur lors de l'assignation du rôle.";
            }
            
            return RedirectToAction(nameof(ManageUserRoles), new { userId });
        }

        // Retirer un rôle à un utilisateur
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var success = await _userService.RemoveRoleFromUserAsync(userId, roleName);
            
            if (success)
            {
                TempData["SuccessMessage"] = $"Rôle {roleName} retiré avec succès.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erreur lors de la suppression du rôle.";
            }
            
            return RedirectToAction(nameof(ManageUserRoles), new { userId });
        }
    }
}