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
        public DbSet <SupportRequest> SupportRequests { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GameTag>()
                .HasKey(gt => new { gt.GameId, gt.TagId });

            modelBuilder.Entity<GameTag>()
                .HasOne(gt => gt.Game)
                .WithMany(g => g.GameTags)
                .HasForeignKey(gt => gt.GameId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<GameTag>()
                .HasOne(gt => gt.Tag)
                .WithMany(t => t.GameTags)
                .HasForeignKey(gt => gt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserGame>()
                .HasKey(ug => new { ug.GameId, ug.UserId });

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.Game)
                .WithMany(g => g.UserGames)
                .HasForeignKey(ug => ug.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.User)
                .WithMany()
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.GameId, od.OrderId });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Game)
                .WithMany(g => g.OrderDetails)
                .HasForeignKey(od => od.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameImage>()
                .HasKey(gi => new { gi.GameId, gi.imageURL }); 


            modelBuilder.Entity<UserRating>()
                .HasKey(ur => ur.RatingId);

            modelBuilder.Entity<UserRating>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<UserRating>()
                .HasOne(ur => ur.Game)
                .WithMany(g => g.UserRatings)
                .HasForeignKey(ur => ur.GameId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Game>()
                .HasOne(g => g.Publisher)
                .WithMany()
                .HasForeignKey(g => g.PublisherId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<GameRequest>()
                .HasOne(gr => gr.Game)  
                .WithMany(g => g.GameRequest); 
            modelBuilder.Entity<GameRequest>()
                .HasOne(gr => gr.Game)  
                .WithMany(g => g.GameRequest)  
                .HasForeignKey(gr => gr.GameId)  
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<RequestTag>()
                .HasKey(rt => new { rt.RequestId, rt.TagId });  

            modelBuilder.Entity<RequestTag>()
                .HasOne(rt => rt.GameRequest)
                .WithMany(gr => gr.Tags)
                .HasForeignKey(rt => rt.RequestId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<RequestTag>()
                .HasOne(rt => rt.Tag)
                .WithMany(t => t.RequestTags)
                .HasForeignKey(rt => rt.TagId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<RequestImage>()
                .HasKey(ri => new { ri.RequestId, ri.ImageUrl }); 
        }

    }
}
