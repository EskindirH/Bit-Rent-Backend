using System.ComponentModel.DataAnnotations;

namespace BitRent.ViewModel
{
    public class EditEmployeeViewModel
    {
        public string Id { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Lastname { get; set; }

        [StringLength(50), EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber {  get; set; }
       
    }
}
