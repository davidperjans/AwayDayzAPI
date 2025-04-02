using AutoMapper;
using AwayDayzAPI.DTOs;
using AwayDayzAPI.Models;
using AwayDayzAPI.Services.Token;
using AwayDayzAPI.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AwayDayzAPI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterDto> _registerValidator;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, IConfiguration config, IMapper mapper, IValidator<RegisterDto> registerValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _config = config;
            _mapper = mapper;
            _registerValidator = registerValidator;
        }
        public async Task<OperationResult<string>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.Username);

                if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                    return OperationResult<string>.Failure("Invalid username or password");

                var token = await _tokenService.GenerateJwtToken(user);
                return OperationResult<string>.Success(token);
            }
            catch (Exception ex)
            {
                // Add loggig (TODO)
                Console.WriteLine($"Login failed: {ex.Message}");
                return OperationResult<string>.Failure("An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<OperationResult<string>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var validationResult = await _registerValidator.ValidateAsync(registerDto);
                if (!validationResult.IsValid)
                    return OperationResult<string>.Failure(validationResult.Errors.First().ErrorMessage);

                var doesUserExists = await _userManager.FindByNameAsync(registerDto.UserName);
                if (doesUserExists != null)
                    return OperationResult<string>.Failure("User already exists");

                var newUser = _mapper.Map<User>(registerDto);
                var result = await _userManager.CreateAsync(newUser, registerDto.Password);

                if (!result.Succeeded)
                    return OperationResult<string>.Failure("Registration failed");

                var token = await _tokenService.GenerateJwtToken(newUser);

                // Assign every new user with role "User"
                await _userManager.AddToRoleAsync(newUser, "User");

                return OperationResult<string>.Success(token);
            }
            catch (Exception ex)
            {
                // Add loggig (TODO)
                Console.WriteLine($"Registration failed: {ex.Message}");
                return OperationResult<string>.Failure("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
