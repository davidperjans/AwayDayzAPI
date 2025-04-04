using AwayDayzAPI.Models.DTOs.Stadium;

namespace AwayDayzAPI.Models.Responses
{
    public class StadiumApiResponse
    {
        public int Results { get; set; }
        public List<ResponseStadium> Response { get; set; }

        public class ResponseStadium
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public int Capacity { get; set; }
            public string Surface { get; set; }
            public string Image { get; set; }
        }
    }
}
