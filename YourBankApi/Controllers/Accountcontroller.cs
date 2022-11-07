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
    public class Accountcontroller : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private IMapper _mapper;
        public Accountcontroller(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        //register New Account
        [HttpPost]
        [Route("register_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(newAccount);
            }

            //map to account
            var account = _mapper.Map<Account>(newAccount);
            return Ok(_accountRepository.Create(account, newAccount.Pin, newAccount.ConfirmPin));
        }

        [HttpGet]
        [Route("get_all_accounts")]
        public IActionResult GetAllAccounts()
        {
            var allAccounts = _accountRepository.GetAllAccounts();
            var getCleanedAccounts = _mapper.Map<IList<GetAccountModel>>(allAccounts);
            return Ok(getCleanedAccounts);
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            //Map
            return Ok(_accountRepository.Authenticate(model.AccountNumber, model.Pin));
        }

        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetByAccountNumber(string AccounNumber)
        {
            if (!Regex.IsMatch(AccounNumber, @"^[0-9]{10}$"))
            {
                return BadRequest("Account Number must be 10 digits");
            }

            var account = _accountRepository.GetByAccountNumber(AccounNumber);
            var cleanedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(cleanedAccount);
        }

        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetAccountById(int Id)
        {
            var account = _accountRepository.GetById(Id);
            var cleanedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(cleanedAccount);
        }

        [HttpPut]
        [Route("update_account")]
        public IActionResult updateAccount([FromBody] UpdateAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var account = _mapper.Map<Account>(model);
            _accountRepository.Update(account, model.Pin);
            return Ok();
        }

        //[HttpPut("{id}")]
        //[Route("update_account")]
        //public IActionResult UpdateAccount(int id, UpdateAccountModel model)
        //{
        //    if (_accountRepository.AccountExists(id))
        //    {
        //        return NotFound();
        //    }

        //    var accountFromRepo = _accountRepository.GetById(id);
        //    if (accountFromRepo == null)
        //    {
        //        return NotFound();
        //    }

        //    var account = _mapper.Map<Account>(model);
        //     _accountRepository.Update(account, model.Pin);
        //     return Ok();


        //}


    }
}
