using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YourBankApi.Models
{
    public class RegisterNewAccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
     //   public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
       // public decimal CurrentAccountBalance { get; set; }

        public AccountType AccountType { get; set; }
        //public string AccountNumberGenerated { get; set; }

        //store the hash and salt of the Account transaction pin
      //  public byte[] PinHash { get; set; }
        //public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }
    }
}
