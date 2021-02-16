using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoOwnershipManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // building is linked to an address
            modelBuilder.Entity<Address>()
                .HasOne<Building>(a => a.Building)
                .WithOne(b => b.Address);

            // building has many appartments
            modelBuilder.Entity<Building>()
                .HasMany<Apartment>(b => b.Apartments)
                .WithOne(a => a.Building)
                .HasForeignKey(a => a.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            // building has many posts
            modelBuilder.Entity<Building>()
                .HasMany<Post>(b => b.Posts)
                .WithOne(p => p.Building)
                .HasForeignKey(p => p.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appartment has many unhabitants
            modelBuilder.Entity<Apartment>()
                .HasMany<ApplicationUser>(a => a.Unhabitants)
                .WithOne(u => u.Apartment)
                .HasForeignKey(u => u.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // User has many posts
            modelBuilder.Entity<ApplicationUser>()
                .HasMany<Post>(a => a.Posts)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
