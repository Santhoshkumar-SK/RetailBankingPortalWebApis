using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Models
{
    public class TransactionDTO
    {
        public int AccountId { get; set; }

        public Double Amount { get; set; }
    }
}
