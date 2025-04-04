namespace AwayDayzAPI.Models
{
    public class Stadium
    {
        public int Id { get; set; } // Primärnyckel
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Capacity { get; set; }
        public string Surface { get; set; }
        public string ImageUrl { get; set; }

        public virtual List<Post> Posts { get; set; } = new List<Post>();

        public double GetAverageRating()
        {
            var ratings = Posts.Select(p => p.Rating).Where(r => r > 0).ToList();
            return ratings.Any() ? ratings.Average() : 0;
        }
    }
}
