using BitRent.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitRent.ViewModel
{
    public class CreatePropertyViewModel
    {        
        public string Description { get; set; } = null!;
        public float Price { get; set; }
        public bool IsAvailable { get; set; }
        
        public IFormFile Photo { get; set; }
        public string? OwnerId { get; set; }
        public string? CustomerId { get; set; }

    }
}
