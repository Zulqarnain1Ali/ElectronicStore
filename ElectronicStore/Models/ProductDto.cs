using System.ComponentModel.DataAnnotations;

namespace ElectronicStore.Models
{
    public class ProductDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Brand { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        [MaxLength(100)]    
        public string? Description { get; set; }

        [MaxLength(100)]
        public IFormFile? ImageFile { get; set; }
    }
}
