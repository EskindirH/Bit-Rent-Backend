﻿using System.ComponentModel.DataAnnotations;

namespace BitRent.Models
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]

        public string? Password { get; set; }

        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
