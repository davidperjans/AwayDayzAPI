using AwayDayzAPI.Models.DTOs.Stadium;

namespace AwayDayzAPI.Services.FootballApiService
{
    public interface IFootballApiService
    {
        Task<List<StadiumDto>> GetStadiumInfoAsync(string stadiumName);
    }
}
