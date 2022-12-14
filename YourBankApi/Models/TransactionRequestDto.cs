using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourBankApi.Models
{
    public class TransactionRequestDto
    {
        public decimal TransactionAmount { get; set; }
           
        public string TransactionSourceAccount { get; set; }

        public string TransactionDestinationAccount { get; set; }

        public TranType TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
