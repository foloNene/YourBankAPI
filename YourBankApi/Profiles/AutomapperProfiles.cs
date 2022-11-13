using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBankApi.Models;
using YourBankApi.Entities;

namespace YourBankApi.Profiles
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<RegisterNewAccountModel, Account>();

            CreateMap<Account, AccountDto>();

            CreateMap<UpdateAccountModel, Account>();

            CreateMap<Account, GetAccountModel>();

            CreateMap<TransactionRequestDto, Transaction>();

            CreateMap<Transaction, TransactionDto>();
        }
    }
}
