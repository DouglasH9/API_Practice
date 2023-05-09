using System;
using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                    new Villa()
                    {
                        Id = 1,
                        Name = "Doug and Kirby's Place",
                        Details = "A cozy place to rest your bones",
                        ImageUrl = "https://www.samslist.us/wp-content/uploads/2020/11/cool-house-pool-arq-pinterest_63660.jpg",
                        Occupancy = 2,
                        Rate = 1000,
                        SquareFt = 500,
                        Amenity = "",

                    },
                    new Villa()
                    {
                        Id = 2,
                        Name = "The Chateau",
                        Details = "Luxurious accomodations for luxurious people.",
                        ImageUrl = "https://cdn.home-designing.com/wp-content/uploads/2020/04/unique-house-design.jpg",
                        Occupancy = 20,
                        Rate = 10000,
                        SquareFt = 20000,
                        Amenity = "",

                    }
                );
        }

    }
}

