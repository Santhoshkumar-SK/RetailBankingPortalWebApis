using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionMicroservice.Models;

namespace TransactionMicroservice.Repository
{
    public interface ITransactionsRepo
    {
        public Task<TransactionStatusForDepositAndWithDraw> deposit(TransactionDTO deposit);

        public Task<TransactionStatusForDepositAndWithDraw> withDraw(TransactionDTO deposit);

        public Task<TransactionSuccess> transferAmount(TransferDTO transfer);

        public TransactionSuccess generateTransactionSuccess(TransactionStatusForDepositAndWithDraw depositStatus, TransactionStatusForDepositAndWithDraw withdrawStatus, TransferDTO transfer);

        public Task<List<TransactionHistory>> getTransactionHistory(int customerId);

        
    }
}
