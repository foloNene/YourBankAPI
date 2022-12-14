using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBankApi.Models;

namespace YourBankApi.Services
{
    public interface ITransactionRepository
    {
        Response CreateNewTransaction(Transaction transaction);

        Response FindTransactionByDate(DateTime date);

        Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);

        Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);

        Response MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);



    }
}
