using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesMicroService.Models
{
    public class Transactions
    {
    }

    public class TransactionStatusForDepositAndWithDraw
    {
        public string Message { get; set; }

        public Double CurrentBalance { get; set; }
    }

    public class TransactionDTO
    {
        public int AccountId { get; set; }

        public Double Amount { get; set; }
    }
}
