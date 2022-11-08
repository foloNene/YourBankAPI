using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourBankApi.DAL;
using YourBankApi.Models;

namespace YourBankApi.Services
{
    public class AccountRepository : IAccountRepository 
    {
        private readonly YourBankingDbContext _dbcontext;
        public AccountRepository(YourBankingDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public Account Authenticate(string AccountNmuber, string Pin)
        {
            //authenticate
            if (string.IsNullOrEmpty(AccountNmuber) || string.IsNullOrEmpty(Pin))
            {
                return null;
            }
            //does account exist for that number
            var account = _dbcontext.Accounts.Where(x => x.AccountNumberGenerated == AccountNmuber).SingleOrDefault();
            if (account == null)
            {
                return null;
            }

            //so if we have a match
            //verify pinHash
            if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
            {
                return null;
            }

            //Aunthication is passed
            return account;

        }

        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrEmpty(Pin))
            {
                throw new ArgumentNullException("Pin");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));

                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            //Check if pin isn't empty.
            if (string.IsNullOrWhiteSpace(Pin))
            {
                throw new ArgumentNullException("Pin cannot be empty");
            }
            //Create new Account
            if (_dbcontext.Accounts.Any(x => x.Email == account.Email))
            {
                throw new ApplicationException("An account already exist with this email");
            }
            //Validate Pin
            if (!Pin.Equals(ConfirmPin))
            {
                throw new ArgumentException("Pins do not match", "Pin");
            }

            //Hashing pin
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);

            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

            //it can be added to db having crypto
            _dbcontext.Accounts.Add(account);
            _dbcontext.SaveChanges();

            return account;

        }

        //Crypto method
        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(pin))
            {
                throw new ArgumentNullException("pin");
            }
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public void Delete(int Id)
        {
            var account = _dbcontext.Accounts.Find(Id);
            if (account != null)
            {
                _dbcontext.Accounts.Remove(account);
                _dbcontext.SaveChanges();
            }
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _dbcontext.Accounts.ToListAsync();
        }

        public  Account GetByAccountNumber(string AccounNumber)
        {
            var account = _dbcontext.Accounts.Where(x => x.AccountNumberGenerated == AccounNumber).SingleOrDefault();

            if (account == null)
            {
                return null;
            }

            return account;
        }



        public async Task <Account> GetByIdAsync(int Id)
        {
            var account =  await _dbcontext.Accounts.FirstOrDefaultAsync(x => x.Id == Id);

            if (account == null)
            {
                return null;
            }
            return account;
        }

        public void Update(Account account, string Pin = null)
        {
            // fnd userr
            var accountToBeUpdated = _dbcontext.Accounts.Find(account.Id);
            if (accountToBeUpdated == null) throw new ApplicationException("Account not found");
            ////so we have a match
            if (!string.IsNullOrWhiteSpace(account.Email) && account.Email != accountToBeUpdated.Email)
            {
                //throw error because email passeed doesn't matc wiith
                if (_dbcontext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("Email " + account.Email + " has been taken");
                accountToBeUpdated.Email = account.Email;
            }

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber) && account.Email != accountToBeUpdated.PhoneNumber)
            {
                //throw error because email passeed doesn't matc wiith
                if (_dbcontext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException("PhoneNumber " + account.PhoneNumber + " has been taken");
                accountToBeUpdated.PhoneNumber = account.PhoneNumber;
            }


            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                accountToBeUpdated.PinHash = pinHash;
                accountToBeUpdated.PinSalt = pinSalt;
            }

            _dbcontext.Accounts.Update(accountToBeUpdated);
            _dbcontext.SaveChanges();

        }

        //To save Trade
        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbcontext.SaveChangesAsync() > 0);
        }

        public bool AccountExists(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _dbcontext.Accounts.Any(a => a.Id == id);
        }
    }
}
