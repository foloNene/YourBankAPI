using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YourBankApi.Models
{
    public class AuthenticateModel
    {
        [Required]
        [RegularExpression(@"^[0-9]{10}$")]
        public string AccountNumber { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must be 4-digit")]
        public string Pin { get; set; }
    } 
}
