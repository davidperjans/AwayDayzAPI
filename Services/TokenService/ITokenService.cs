using AwayDayzAPI.Models;
using AwayDayzAPI.Utils;

namespace AwayDayzAPI.Services.Token
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
