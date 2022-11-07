using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBankApi.Models;

namespace YourBankApi.DAL
{
    public class YourBankingDbContext : DbContext
    {
        public YourBankingDbContext(DbContextOptions<YourBankingDbContext> options) : base(options)
        {

        }

        //dbset
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }


    }
}
