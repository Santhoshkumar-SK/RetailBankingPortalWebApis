using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionMicroservice.Exceptions
{
    public class TransferException : Exception
    {
        public TransferException(string message) : base(message)
        {

        }
    }
}
