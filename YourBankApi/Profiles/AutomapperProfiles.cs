using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBankApi.Models;

namespace YourBankApi.Profiles
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<RegisterNewAccountModel, Account>();

            CreateMap<UpdateAccountModel, Account>();

            CreateMap<Account, GetAccountModel>();

            CreateMap<TransactionRequestDto, Transaction>();
        }
    }
}
