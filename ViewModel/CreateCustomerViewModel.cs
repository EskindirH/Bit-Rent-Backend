using System.ComponentModel.DataAnnotations;

namespace BitRent.ViewModel
{
    public class CreateCustomerViewModel
    {
        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Lastname { get; set; }

        [StringLength(50), EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public string Address { get; set; } = null!;

        [StringLength(50), DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Date)]
        public DateTime EnrollmentDate
        {
            get; set;
        }
}
}
