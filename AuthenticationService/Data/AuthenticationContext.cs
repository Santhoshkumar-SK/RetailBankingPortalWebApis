using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Data
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext()
        {

        }

        public AuthenticationContext(DbContextOptions opts) : base(opts)
        {

        }

        public DbSet<User> Authentication { get; set; }
    }
}
