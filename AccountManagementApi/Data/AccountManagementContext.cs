using AccountManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Data
{
    public class AccountManagementContext : DbContext
    {
        public AccountManagementContext()
        {

        }

        public AccountManagementContext(DbContextOptions opts) : base(opts)
        {

        }

        public DbSet<Account> Account { get; set; }

        public DbSet<AccountCreationStatus> AccountCreationStatus { get; set; }
        public DbSet<Statement> Statement { get; set; }
        public DbSet<TransactionStatus> TransactionStatus { get; set; }


    }
}
