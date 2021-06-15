using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionMicroservice.Models;

namespace TransactionMicroservice.Services
{
    public interface ITransactionsService
    {
        public Task<TransactionStatusForDepositAndWithDraw> depositAsync(TransactionDTO deposit);

        public RulesStatus checkRulesForWithDraw(TransactionDTO amount);

        public Account getAccountBalance(int accountId);

        public Task<TransactionStatusForDepositAndWithDraw> withdrawAsync(TransactionDTO deposit);

        public List<Account> getAllaccounts();

        public string generateJSONWebToken();
    }
}
