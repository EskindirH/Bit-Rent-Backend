namespace BitRent.Models
{
    public class Customer : AppUser
    {
        public string AccountNumber { get; set; } = null!;

        public ICollection<Property>? Properties { get; set; }
        public ICollection<BitTransaction>? BitTransaction { get; set; }
    }
}
