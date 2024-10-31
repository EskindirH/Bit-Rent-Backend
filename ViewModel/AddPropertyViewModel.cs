using BitRent.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitRent.ViewModel
{
    public class AddPropertyViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public bool IsAvailable { get; set; }
         
        public IFormFile? Photo { get; set; }
        public byte[]? PhotoFile { get; set; }
        public string? OwnerId { get; set; }
        public string? CustomerId { get; set; }
    }
}
