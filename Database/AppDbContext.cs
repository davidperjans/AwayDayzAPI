using AwayDayzAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AwayDayzAPI.Database
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ändra standardtabellnamnen för Identity
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            // Configure the relationships between ApplicationUser and FriendRequest
            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            // Configure the relationships between ApplicationUser and Friendship (User1 and User2)
            builder.Entity<Friendship>()
                .HasOne(f => f.User1)
                .WithMany(u => u.FriendshipsAsUser1)  // Changed to FriendshipsAsUser1
                .HasForeignKey(f => f.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friendship>()
                .HasOne(f => f.User2)
                .WithMany(u => u.FriendshipsAsUser2)  // Changed to FriendshipsAsUser2
                .HasForeignKey(f => f.User2Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
