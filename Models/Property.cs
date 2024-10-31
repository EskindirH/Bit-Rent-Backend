using System.ComponentModel.DataAnnotations.Schema;

namespace BitRent.Models
{ 
    public class Property
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = null!;
        public string Description { get; set; } = null!;
        public float Price { get; set; }
        public bool IsAvailable { get; set; }
        public string FilePath { get; set; } = null!;
        public string? OwnerId { get; set; }
        public string? CustomerId { get; set; }

        public Customer? Customer { get; set; } 
        public Owner? Owner { get; set; }
    }
}
