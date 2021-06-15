using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions opts) : base(opts)
        {

        }
        public CustomerContext() { }
        public DbSet<Customer> customers { get; set; }
    }
}
