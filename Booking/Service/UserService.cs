using System.Collections.Generic;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.AspNetCore.Identity;
using Solution.Models;

namespace Solution.Service;

 public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (!await _roleManager.RoleExistsAsync(roleName))
                return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<bool> PromoteToHostAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Retirer le rôle Guest s'il existe
            if (await _userManager.IsInRoleAsync(user, "Guest"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Guest");
            }

            // Ajouter le rôle Host
            var result = await _userManager.AddToRoleAsync(user, "Host");
            return result.Succeeded;
        }

        public async Task<bool> DemoteFromHostAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Retirer le rôle Host
            await _userManager.RemoveFromRoleAsync(user, "Host");

            // Ajouter le rôle Guest
            var result = await _userManager.AddToRoleAsync(user, "Guest");
            return result.Succeeded;
        }
        
        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

    }
