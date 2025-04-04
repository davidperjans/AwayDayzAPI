using AwayDayzAPI.Services.FootballApiService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwayDayzAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController : ControllerBase
    {
        private readonly IFootballApiService _footballApiService;

        public StadiumController(IFootballApiService footballApiService)
        {
            _footballApiService = footballApiService;
        }

        [HttpGet("stadium/{stadiumName}")]
        public async Task<IActionResult> GetStadiumInfo(string stadiumName)
        {
            var stadiumInfo = await _footballApiService.GetStadiumInfoAsync(stadiumName);

            if (stadiumInfo == null)
                return NotFound("Ingen arena hittades");

            return Ok(stadiumInfo);
        }
    }
}
