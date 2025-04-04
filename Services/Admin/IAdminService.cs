using AwayDayzAPI.Utils;
using Microsoft.AspNetCore.Identity;

namespace AwayDayzAPI.Services.Admin
{
    public interface IAdminService
    {
        Task<OperationResult<string>> AssignRoleAsync(string userId, string roleName);
        Task<OperationResult<List<IdentityRole>>> GetAllRolesAsync(string sortOrder);
    }
}
