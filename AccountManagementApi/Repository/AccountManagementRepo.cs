using AccountManagementApi.Data;
using AccountManagementApi.Enum;
using AccountManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Repository
{
    public class AccountManagementRepo : IAccountManagementRepo
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AccountManagementRepo));

        private AccountManagementContext _context;

        private ForgettingDates _date;

        public AccountManagementRepo(AccountManagementContext context)
        {
            _context = context;
            _date = new ForgettingDates();
        }

        public async Task<AccountCreationStatus> createAccountForCustomer(Account account)
        {
            AccountCreationStatus creationStatus = null;

            try
            {
                
                       _context.Account.Add(account);

                      await _context.SaveChangesAsync();

                    creationStatus = addStatusForAccountCreation(account.AccountID, "Account Created");

                    addStatementforalltransactions(account.AccountID, "Deposited", account.AccountBalance);

            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);

                creationStatus = null;
            }

            _log4net.Info("Account Created for Customer Successfully");

            return creationStatus;
        }

        public AccountCreationStatus addStatusForAccountCreation(int accountId , string message)
        {
            AccountCreationStatus creationStatus = new AccountCreationStatus() {
                AccountID = accountId,
                Message = message
            };

            try 
            {
                _context.AccountCreationStatus.Add(creationStatus);

                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
            }

            return creationStatus;
        }

        public async Task<List<AccountDetailsForSending>> getCustomerAccounts(int customerId)
        {
            List<AccountDetailsForSending> accounts = new List<AccountDetailsForSending>();

            try
            {
                 accounts = await  _context.Account.Where(custId => custId.CustomerID == customerId).Select(toselect => new AccountDetailsForSending{ 
                    AccountId = toselect.AccountID,
                    AccountBalance = toselect.AccountBalance
                }).ToListAsync();
                
                if(accounts == null)
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);

                return null;
            }

            _log4net.Info("Accounts of the  Customer is fetched  Successfully");
            return accounts;
        }

        public async Task<AccountDetailsForSending> getAccount(int accountId)
        {
            AccountDetailsForSending accountDetail = null;

            try
            {
                accountDetail = await _context.Account.Where(accId => accId.AccountID == accountId).Select(toselect => new AccountDetailsForSending
                {
                    AccountId = toselect.AccountID,
                    AccountBalance = toselect.AccountBalance
                }).FirstOrDefaultAsync();
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);

                return new AccountDetailsForSending();
            }

            _log4net.Info("Accounts Details of the given account id is fetched  Successfully");

            return accountDetail;
        }


        public async Task<TransactionStatusForDepositAndWithDraw> deposit(TransactionDTO transactionDTO)
        {
            TransactionStatusForDepositAndWithDraw depositStatus = null;

            try
            {
                var todeposit = await _context.Account.Where(accId => accId.AccountID == transactionDTO.AccountId).FirstOrDefaultAsync();

                if(todeposit != null)
                {
                    todeposit.AccountBalance = todeposit.AccountBalance + transactionDTO.Amount;
                    _context.SaveChanges();

                    _log4net.Info("Given amount is deposited Successfully");

                    addStatementforalltransactions(transactionDTO.AccountId,"Deposited",transactionDTO.Amount);

                    depositStatus = forGivingthestatusofdepositorwithdraw(true, todeposit.AccountBalance, "Deposit");
                    
                }    
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                return null;
            }

            return depositStatus;
        }


        public TransactionStatusForDepositAndWithDraw forGivingthestatusofdepositorwithdraw(bool valid , Double amount , string depositorwithdraw)
        {
            if (valid)
            {
                TransactionStatusForDepositAndWithDraw status = new TransactionStatusForDepositAndWithDraw()
                {
                    Message = depositorwithdraw + " Successfull",
                    CurrentBalance = amount
                };

                return status;
            }

            return new TransactionStatusForDepositAndWithDraw() { Message = depositorwithdraw+ " Failed", CurrentBalance = amount };
        }

        public  void addStatementforalltransactions(int accountId , string withdrawOrDeposit , Double closingBalance)
        {
            try
            {
                Statement statement = new Statement()
                {
                    AccountID = accountId,
                    ClosingBalance = closingBalance,
                    Date = _date.currentDate(),
                    WithdrawalOrDeposit = withdrawOrDeposit
                };

                _context.Statement.Add(statement);
                _context.SaveChanges();

                _log4net.Info("Statement for the transaction is added");
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                return;
            }
        }

        public async Task<List<Statement>> getAccountStatement(int accountId, DateTime from_date, DateTime to_date)
        {
            List<Statement> statement = null;
            try
            {
                if (ifAccountExists(accountId))
                {

                    _log4net.Info("Statement for the transaction is fetched Successfully");

                    statement = await _context.Statement.Where(accId => accId.AccountID == accountId && (accId.Date >= from_date && accId.Date <= to_date)).ToListAsync();

                    return statement;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                
            }

            return statement;
        }

        public async Task<TransactionStatusForDepositAndWithDraw> withdraw(TransactionDTO transactionDTO)
        {
            TransactionStatusForDepositAndWithDraw withdrawStatus = null;
            try
            {
                var toWithdraw = await _context.Account.Where(accId => accId.AccountID == transactionDTO.AccountId).FirstOrDefaultAsync();

                if(toWithdraw != null)
                {
                    toWithdraw.AccountBalance = toWithdraw.AccountBalance - transactionDTO.Amount;
                    _context.SaveChanges();

                    _log4net.Info("Given amount is deposited Successfully");

                    addStatementforalltransactions(transactionDTO.AccountId, "Withdrawn", transactionDTO.Amount);

                    withdrawStatus = forGivingthestatusofdepositorwithdraw(true, toWithdraw.AccountBalance, "Withdraw");

                }
               
            }
            catch ( Exception exception)
            {

                _log4net.Error(exception.Message);
                return null;
            }

            return withdrawStatus;
        }

        public Boolean ifAccountExists(int accountId)
        {
            try
            {
                return _context.Account.Any(accId => accId.AccountID == accountId);
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                return false;
            }
        }

        public async Task<List<Account>> getAllCustomerAccounts()
        {
            try
            {
                return await _context.Account.ToListAsync();
            }
            catch(Exception exception)
            {
                _log4net.Error(exception.Message);
                return new List<Account>();
            }
        }
    }
}
