namespace BitRent.Models
{
    public class Owner : AppUser
    {
        public string AccountNumber { get; set; } = null!;
        
        public ICollection<Property>? Properties { get; set; }
        public ICollection<BitTransaction>? Transactions { get; set; }        
    }
}
