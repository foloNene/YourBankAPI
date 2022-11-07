using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBankApi.Models;

namespace YourBankApi.Services
{
    public interface IAccountRepository
    {
        Account Authenticate(string AccountNmuber, string Pin);

        IEnumerable<Account> GetAllAccounts();

        Account Create(Account account, string Pin, string ConfirmPin);

        void Update(Account account, string Pin = null);

        void Delete(int Id);

        Account GetById(int Id);

        Account GetByAccountNumber(string AccounNumber);

        bool AccountExists(int id);
    }
}
