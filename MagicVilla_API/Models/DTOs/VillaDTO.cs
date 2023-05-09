using System;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTOs
{
    public class VillaDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Name { get; set; }

        public string? Details { get; set; }

        [Required]
        public double Rate { get; set; }

        public string? ImageUrl { get; set; }

        public string? Amenity { get; set; }

        [Required]
        public uint Occupancy { get; set; }

        public int SquareFt { get; set; }

    }
}

