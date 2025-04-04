using AwayDayzAPI.Models;
using Bogus;
using Microsoft.AspNetCore.Identity;

namespace AwayDayzAPI.Seeder
{
    public class DatabaseSeeder
    {
        private readonly UserManager<User> _userManager;

        public DatabaseSeeder(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedUsersAsync()
        {
            // Kolla om användarna finns i databasen
            if (!_userManager.Users.Any())
            {
                var faker = new Faker<User>()
                    .RuleFor(u => u.UserName, f => f.Internet.UserName())  // Slumpmässigt användarnamn
                    .RuleFor(u => u.Email, f => f.Internet.Email())        // Slumpmässig e-postadress
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())     // Förnamn
                    .RuleFor(u => u.LastName, f => f.Name.LastName());       // Efternamn

                var user = faker.Generate();

                var result = await _userManager.CreateAsync(user, "Password123!"); // Ange ett lösenord

                if (result.Succeeded)
                {
                    Console.WriteLine($"User {user.UserName} created successfully!");
                }
                else
                {
                    Console.WriteLine($"Error creating user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine("Database already contains users. Skipping seeding.");
            }
        }
    }
}
