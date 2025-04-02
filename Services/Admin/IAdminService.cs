using AwayDayzAPI.Utils;

namespace AwayDayzAPI.Services.Admin
{
    public interface IAdminService
    {
        Task<OperationResult<string>> AssignRoleAsync(string userId, string roleName);
    }
}
