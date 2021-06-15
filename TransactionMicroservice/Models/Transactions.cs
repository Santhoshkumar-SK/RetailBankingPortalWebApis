using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionMicroservice.Models
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

    public class TransferDTO
    {
        public int SourceAccountId { get; set; }

        public int DestinationAccountId { get; set; }

        public double Amount { get; set; }
    }

    public class TransactionSuccess
    {
        public string message { get; set; }

        public double SourceAccountBalance { get; set; }

        public double DestinationBalance { get; set; }
    }
}
