using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBankApi.DAL;
using YourBankApi.Models;
using YourBankApi.Utils;

namespace YourBankApi.Services
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly YourBankingDbContext _dbcontext;
        private readonly ILogger<TransactionRepository> _logger;
        private AppSettings _settings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountRepository _accountRepository;

        public TransactionRepository(YourBankingDbContext dbcontext, 
            ILogger<TransactionRepository> logger, 
            IOptions<AppSettings> settings,
            IAccountRepository accountRepository)
        {
            _dbcontext = dbcontext;
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountRepository = accountRepository;

        }

        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _dbcontext.Transactions.Add(transaction);
            _dbcontext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully!";
            response.Data = null;
            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _dbcontext.Transactions.Where(x => x.TransactionDate == date).ToList();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successfully!";
            response.Data = transaction;

            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //Make a deposit....
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //first check that user account owner is valid
            //aunthenticate in UserService, by injecting IUserService here
            var authUser = _accountRepository.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null)
            {
                throw new ApplicationException("Invalid credential");
            }

            //s validation passes
            try
            {
                sourceAccount = _accountRepository.GetByAccountNumber(_ourBankSettlementAccount);
                destinationAccount = _accountRepository.GetByAccountNumber(AccountNumber);

                //let's update their account balance
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is updates
                if ((_dbcontext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbcontext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfully";
                    response.Data = null; 
                }
                else
                {
                    //so transaction is unsuccessful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURERD..=>{ ex.Message}");

            }

            //set other props of transaction here
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionType = TranType.Deposit;
            transaction.TransactionAmount = Amount;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionParticulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON {transaction.TransactionDate} " +
                $"TRAN_TYPE =>  {transaction.TransactionType} TRAN_STATUS => {transaction.TransactionStatus}";

            _dbcontext.Transactions.Add(transaction);
            _dbcontext.SaveChanges();

            return response;


        }

        public Response MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            //let's implement withdrawal now...

            //make withdraw...
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //first check that user account owner is valid
            //aunthenticate in UserService, by injecting IUserService here
            var authUser = _accountRepository.Authenticate(FromAccount, TransactionPin);
            if (authUser == null)
            {
                throw new ApplicationException("Invalid credential");
            }

            //s validation passes
            try
            {
                //for deposit, our banksettlement is the distination getting money from the user's account
                sourceAccount = _accountRepository.GetByAccountNumber(FromAccount);
                destinationAccount = _accountRepository.GetByAccountNumber(ToAccount);

                //let's update their account balance
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is updates
                if ((_dbcontext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbcontext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfully";
                    response.Data = null;
                }
                else
                {
                    //so transaction is unsuccessful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURERD..=>{ ex.Message}");

            }

            //set other props of transaction here
            transaction.TransactionType = TranType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
               $"TO DESTINATION => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON {transaction.TransactionDate} " +
               $"TRAN_TYPE =>  {transaction.TransactionType} TRAN_STATUS => {transaction.TransactionStatus}";

            _dbcontext.Transactions.Add(transaction);
            _dbcontext.SaveChanges();

            return response;

        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //let's implement withdrawal now...

            //make withdraw...
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //first check that user account owner is valid
            //aunthenticate in UserService, by injecting IUserService here
            var authUser = _accountRepository.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null)
            {
                throw new ApplicationException("Invalid credential");
            }

            //s validation passes
            try
            {
                //for deposit, our banksettlement is the distination getting money from the user's account
                sourceAccount = _accountRepository.GetByAccountNumber(AccountNumber);
                destinationAccount = _accountRepository.GetByAccountNumber(_ourBankSettlementAccount);

                //let's update their account balance
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is updates
                if ((_dbcontext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbcontext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //so transaction is successful
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfully";
                    response.Data = null;
                }
                else
                {
                    //so transaction is unsuccessful
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed";
                    response.Data = null;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURERD..=>{ ex.Message}");

            }

            //set other props of transaction here
            transaction.TransactionType = TranType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber; 
            transaction.TransactionDestinationAccount = _ourBankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} ON {transaction.TransactionDate} " +
                $"TRAN_TYPE =>  {transaction.TransactionType} TRAN_STATUS => {transaction.TransactionStatus}";

            _dbcontext.Transactions.Add(transaction);
            _dbcontext.SaveChanges();

            return response;

        }
    }
}
