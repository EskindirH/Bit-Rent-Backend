using System.ComponentModel.DataAnnotations;

namespace BitRent.ViewModel
{
    public class RegisterUser
    {

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Lastname { get; set; }

        [StringLength(50), EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string? Discriminator { get; set; } 

        public string? AccountNumber { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

    }
}
