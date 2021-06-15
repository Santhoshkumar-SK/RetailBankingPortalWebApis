using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionMicroservice.Models;

namespace TransactionMicroservice.Data
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions opts ) : base(opts)
        {

        }
        public TransactionContext()
        {

        }

        public DbSet<TransactionHistory> TransactionHistory { get; set; }
    }
}
