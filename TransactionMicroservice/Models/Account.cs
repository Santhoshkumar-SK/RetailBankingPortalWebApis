using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionMicroservice.Models
{
    public class Account
    {
        public int AccountID { get; set; }

        public int CustomerID { get; set; }

        public string AccountType { get; set; }

        public Double AccountBalance { get; set; }
    }
}
