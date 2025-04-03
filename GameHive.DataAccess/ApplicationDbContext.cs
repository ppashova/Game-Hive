using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameHive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameHive.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GameTag> GameTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<PublisherRequest> PublisherRequests { get; set; }
        public DbSet<GameImage> GameImages { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<GameRequest> GameRequests { get; set; }
        public DbSet<RequestTag> RequestTags { get; set; }
        public DbSet<RequestImage> RequestImages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Key for GameTag
            modelBuilder.Entity<GameTag>()
                .HasKey(gt => new { gt.GameId, gt.TagId });

            modelBuilder.Entity<GameTag>()
                .HasOne(gt => gt.Game)
                .WithMany(g => g.GameTags)
                .HasForeignKey(gt => gt.GameId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures tags get removed if game is deleted

            modelBuilder.Entity<GameTag>()
                .HasOne(gt => gt.Tag)
                .WithMany(t => t.GameTags)
                .HasForeignKey(gt => gt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            // Composite Key for UserGame
            modelBuilder.Entity<UserGame>()
                .HasKey(ug => new { ug.GameId, ug.UserId });

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.Game)
                .WithMany(g => g.UserGames)
                .HasForeignKey(ug => ug.GameId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures UserGame records are removed if game is deleted

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.User)
                .WithMany()
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Avoids cascade issues with IdentityUser

            // Composite Key for OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.GameId, od.OrderId });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Game)
                .WithMany(g => g.OrderDetails)
                .HasForeignKey(od => od.GameId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures OrderDetails are removed if game is deleted

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Composite Key for GameImage
            modelBuilder.Entity<GameImage>()
                .HasKey(gi => new { gi.GameId, gi.imageURL }); // Fixed property name casing

            // User Ratings Setup
            modelBuilder.Entity<UserRating>()
                .HasKey(ur => ur.RatingId);

            modelBuilder.Entity<UserRating>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents issues with IdentityUser

            modelBuilder.Entity<UserRating>()
                .HasOne(ur => ur.Game)
                .WithMany(g => g.UserRatings)
                .HasForeignKey(ur => ur.GameId)
                .OnDelete(DeleteBehavior.Cascade); // Ensures ratings are deleted when game is deleted

            // 🔥 Fix for Publisher Cascade Delete Issue
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Publisher)
                .WithMany()
                .HasForeignKey(g => g.PublisherId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade error on IdentityUser

            modelBuilder.Entity<GameRequest>()
                .HasOne(gr => gr.Game)  // GameRequest has one Game
                .WithMany(g => g.GameRequest); // Game has one GameRequest
            modelBuilder.Entity<GameRequest>()
                .HasOne(gr => gr.Game)  // GameRequest has one Game
                .WithMany(g => g.GameRequest)  // Game has one GameRequest
                .HasForeignKey(gr => gr.GameId)  // Foreign key in GameRequest
                .OnDelete(DeleteBehavior.Cascade);  // Delete GameRequest if Game is deleted

            modelBuilder.Entity<RequestTag>()
                .HasKey(rt => new { rt.RequestId, rt.TagId });  // Composite key

            modelBuilder.Entity<RequestTag>()
                .HasOne(rt => rt.GameRequest)
                .WithMany(gr => gr.Tags)
                .HasForeignKey(rt => rt.RequestId)
                .OnDelete(DeleteBehavior.Cascade);  // Delete RequestTag if GameRequest is deleted

            modelBuilder.Entity<RequestTag>()
                .HasOne(rt => rt.Tag)
                .WithMany(t => t.RequestTags)
                .HasForeignKey(rt => rt.TagId)
                .OnDelete(DeleteBehavior.Cascade);  // Delete RequestTag if Tag is deleted

            modelBuilder.Entity<RequestImage>()
                .HasKey(ri => new { ri.RequestId, ri.ImageUrl });  // Composite key
        }

    }
}
