﻿using AwayDayzAPI.DTOs;
using AwayDayzAPI.Utils;

namespace AwayDayzAPI.Services.Auth
{
    public interface IAuthService
    {
        Task<OperationResult<string>> RegisterAsync(RegisterDto registerDto);
        Task<OperationResult<string>> LoginAsync(LoginDto loginDto);
    }
}
