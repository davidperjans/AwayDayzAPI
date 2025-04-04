using AwayDayzAPI.Models.DTOs.Admin;
using AwayDayzAPI.Services.Admin;
using AwayDayzAPI.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AwayDayzAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto)
        {

            var result = await _adminService.AssignRoleAsync(assignRoleDto.UserId, assignRoleDto.RoleName);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.Message);
        }

        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetRoles([FromQuery] string sortOrder = "asc")
        {
            var result = await _adminService.GetAllRolesAsync(sortOrder);

            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.Message);
        }
    }
}
