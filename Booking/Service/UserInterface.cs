using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Solution.Models;

namespace Solution.Service
{
    public interface IUserService
    {
        Task<bool> AssignRoleToUserAsync(string userId, string roleName);
        Task<bool> RemoveRoleFromUserAsync(string userId, string roleName);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName);
        Task<bool> PromoteToHostAsync(string userId);
        Task<bool> DemoteFromHostAsync(string userId);
        Task<ApplicationUser?> GetUserByIdAsync(string userId);

    }
}