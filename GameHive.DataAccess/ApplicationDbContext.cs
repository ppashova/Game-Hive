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
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<PublisherRequest> PublisherRequests { get; set; }
        public DbSet<GameImage> GameImages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameTag>()
                .HasKey(gt => new { gt.GameId, gt.TagId });

            modelBuilder.Entity<GameTag>()
                .HasOne(gt => gt.Game)
                .WithMany(g => g.GameTags)
                .HasForeignKey(gt => gt.GameId);

            modelBuilder.Entity<GameTag>()
                .HasOne(gt => gt.Tag)
                .WithMany(t => t.GameTags)
                .HasForeignKey(gt => gt.TagId);

            modelBuilder.Entity<UserGame>()
               .HasKey(gt => new { gt.GameId, gt.UserId });

            modelBuilder.Entity<UserGame>()
                .HasOne(gt => gt.Game)
                .WithMany(g => g.UserGames)
                .HasForeignKey(gt => gt.GameId);

            modelBuilder.Entity<UserGame>()
             .HasOne(ug => ug.Game)
             .WithMany(g => g.UserGames)
             .HasForeignKey(ug => ug.GameId);

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.GameId, od.OrderId });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Game)
                .WithMany(g => g.OrderDetails)
                .HasForeignKey(od => od.GameId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);
            modelBuilder.Entity<GameImage>()
                .HasNoKey();
        }
    }
}
