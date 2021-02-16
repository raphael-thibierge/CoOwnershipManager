﻿using System;
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

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Apartment> Apartments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // building has many appartments
            modelBuilder.Entity<Building>()
                .HasMany<Apartment>(b => b.Apartments)
                .WithOne(a => a.Building)
                .HasForeignKey(a => a.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appartment has many unhabitants
            modelBuilder.Entity<Apartment>()
                .HasMany<ApplicationUser>(a => a.Unhabitants)
                .WithOne(u => u.Apartment)
                .HasForeignKey(a => a.ApartmentId)
                .OnDelete(DeleteBehavior.SetNull); // todo : verify set null
        }
    }
}
