using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YourBankApi.Models;
using YourBankApi.Services;

namespace YourBankApi.Controllers
{
    [ApiController]
    [Route("api/v3/[controller]")]
    public class TransactionContoller : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private IMapper _mapper;

        public TransactionContoller(ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        //Create new transaction
        [HttpPost]
        [Route("creat_new_transaction")]
        public IActionResult CreateNewTransaction([FromBody] TransactionRequestDto transactionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(transactionRequest);
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);

            return Ok(_transactionRepository.CreateNewTransaction(transaction));
        }

        [HttpPost]
        [Route("make_desposit")]
        public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //if (!Regex.IsMatch(AccountNumber, @"^[0-9]{10}$"))
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            return Ok(_transactionRepository.MakeDeposit(AccountNumber, Amount, TransactionPin));
        }

        [HttpPost]
        [Route("make_withdrawal")]
        public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0-9]{10}$"))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            return Ok(_transactionRepository.MakeWithdrawal(AccountNumber, Amount, TransactionPin));
        }

        [HttpPost]
        [Route("make_funds_transfer")]
        public IActionResult MakeFundTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if ((!Regex.IsMatch(FromAccount, @"^[0-9]{10}$")) || (!Regex.IsMatch(ToAccount, @"^[0-9]{10}$")))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            return Ok(_transactionRepository.MakeFundTransfer(FromAccount, ToAccount, Amount, TransactionPin));
            //if (FromAccount.Equals(ToAccount)) return BadRequest("You cannot transfer money to yourself");

            //return Ok(_transactionRepository.MakeFundTransfer(FromAccount, ToAccount, Amount, TransactionPin));
        }
    }
}
