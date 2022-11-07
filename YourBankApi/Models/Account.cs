using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YourBankApi.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }

        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }


        //store the hash and salt of the Account transaction pin
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        //Generate Account Number from the Constructor

        //first, generate random obj
        Random rand = new Random();


        public Account()
        {
            //Generate account number
            AccountNumberGenerated = Convert.ToString((long)Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));

            //also AccountName property = FirstName + LastName;
            AccountName = $"{FirstName} {LastName}";
        }

    }

    public enum AccountType
    {
        Savings,
        Current,
        Cooperate,
        Government
    }
}
