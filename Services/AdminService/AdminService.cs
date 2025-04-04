using AwayDayzAPI.Models;
using AwayDayzAPI.Utils;
using Microsoft.AspNetCore.Identity;

namespace AwayDayzAPI.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<OperationResult<string>> AssignRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return OperationResult<string>.Failure("User not found");

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return OperationResult<string>.Failure("Role does not exist");

            // Remove user from "User" role
            await _userManager.RemoveFromRoleAsync(user, "User");

            // Add user to the specified role
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded
                ? OperationResult<string>.Success("Role assigned successfully")
                : OperationResult<string>.Failure("Failed to assign role");
        }

        public async Task<OperationResult<List<IdentityRole>>> GetAllRolesAsync(string sortOrder)
        {
            var roles = _roleManager.Roles.ToList();
            
            if (sortOrder.ToLower() == "desc")
            {
                roles = roles.OrderByDescending(r => r.Name).ToList();
            }
            else
            {
                roles = roles.OrderBy(r => r.Name).ToList();
            }

            var roleList = roles.ToList();
            return OperationResult<List<IdentityRole>>.Success(roleList);

        }
    }
}
