using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ElectronicStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Brand { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }

        [Precision(14, 2)]
        public decimal Price { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
