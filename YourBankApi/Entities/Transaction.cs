using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YourBankApi.Enums;

namespace YourBankApi.Entities
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        
        public string TransactionUniqueReference { get; set; } //this will generate in every instance off the class
        public decimal TransactionAmount { get; set; }
        public TranStatus TransactionStatus { get; set; }  // An enum
        public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success); //depends on the value of transactionStatus
        public string TransactionSourceAccount { get; set; }

        public string TransactionDestinationAccount { get; set; }

        public string TransactionParticulars { get; set; }

        public TranType TransactionType { get; set; } 

        public DateTime TransactionDate { get; set; }


        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-","").Substring(1, 27)}"; //Generate Refrence with Guid
        }
    }





}
