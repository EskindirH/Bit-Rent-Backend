using System.ComponentModel.DataAnnotations.Schema;

namespace BitRent.Models
{
    public class BitTransaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Amount { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string OwnerId { get; set; } = null!;
        public string CustomerId { get; set; } = null!;

        public Owner Owner { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
    }
}
