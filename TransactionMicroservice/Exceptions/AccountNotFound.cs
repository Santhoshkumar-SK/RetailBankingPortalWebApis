using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionMicroservice.Exceptions
{
    public class AccountNotFound : Exception
    {
        public AccountNotFound(string message): base(message)
        {

        }
    }
}
