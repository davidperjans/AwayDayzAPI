using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AwayDayzAPI.Models;
using AwayDayzAPI.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AwayDayzAPI.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public TokenService(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task<string> GenerateJwtToken(User user)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                var roles = await _userManager.GetRolesAsync(user); // Getting the roles from the user
                var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email!)
                };

                claims.AddRange(roleClaims); // Add roles to claims

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(double.Parse(_config["JWT:ExpireMinutes"]!)),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Add loggig (TODO)
                Console.WriteLine($"Registration failed: {ex.Message}");
                return "An unexpected error occurred. Please try again later.";
            }
        }
    }
}
