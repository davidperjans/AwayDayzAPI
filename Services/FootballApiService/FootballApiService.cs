using AutoMapper;
using AwayDayzAPI.Models.DTOs.Stadium;
using AwayDayzAPI.Models.Responses;
using Newtonsoft.Json;

namespace AwayDayzAPI.Services.FootballApiService
{
    public class FootballApiService : IFootballApiService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public FootballApiService(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _client = new HttpClient();
            _apiKey = configuration.GetValue<string>("ApiSettings:FootballApiKey");
            _client.DefaultRequestHeaders.Add("x-apisports-key", _apiKey);
        }


        public async Task<List<StadiumDto>> GetStadiumInfoAsync(string stadiumName)
        {
            var response = await _client.GetAsync($"https://v3.football.api-sports.io/venues?search={stadiumName}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<StadiumApiResponse>(content);

            var stadiumDtos = _mapper.Map<List<StadiumDto>>(apiResponse.Response);

            return stadiumDtos;
        }
    }
}
