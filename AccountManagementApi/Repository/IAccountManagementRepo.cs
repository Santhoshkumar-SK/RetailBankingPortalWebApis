using AccountManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Repository
{
    public interface IAccountManagementRepo
    {
        public Task<AccountCreationStatus> createAccountForCustomer(Account account);

        public Task<List<AccountDetailsForSending>> getCustomerAccounts(int customerId);

        public Task<AccountDetailsForSending> getAccount(int accountId);

        public Task<TransactionStatusForDepositAndWithDraw> deposit(TransactionDTO transactionDTO);

        public Task<List<Statement>> getAccountStatement(int accountId, DateTime from_date, DateTime to_date);

        public Task<TransactionStatusForDepositAndWithDraw> withdraw(TransactionDTO transactionDTO);

        public Boolean ifAccountExists(int accountId);

        public void addStatementforalltransactions(int accountId, string withdrawOrDeposit, Double closingBalance);

        public TransactionStatusForDepositAndWithDraw forGivingthestatusofdepositorwithdraw(bool valid, Double amount, string depositorwithdraw);

        public AccountCreationStatus addStatusForAccountCreation(int accountId, string message);

        public Task<List<Account>> getAllCustomerAccounts();
    }
}
