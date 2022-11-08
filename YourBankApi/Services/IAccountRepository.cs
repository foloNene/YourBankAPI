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

        Task<IEnumerable<Account>> GetAllAccountsAsync();

        Account Create(Account account, string Pin, string ConfirmPin);

        void Update(Account account, string Pin = null);

        void Delete(int Id);

        Task<Account> GetByIdAsync(int Id);

        Account GetByAccountNumber(string AccounNumber);

        bool AccountExists(int id);

        Task<bool> SaveChangesAsync();
    }
}
