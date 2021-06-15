using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionMicroservice.Data;
using TransactionMicroservice.Exceptions;
using TransactionMicroservice.Models;
using TransactionMicroservice.Services;

namespace TransactionMicroservice.Repository
{
    public class TransactionsRepo : ITransactionsRepo
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TransactionsRepo));
        private readonly ITransactionsService _services;
        private readonly TransactionContext _context;
        Double AccountBalanceForCheck = 0;

        public TransactionsRepo(ITransactionsService services , TransactionContext context)
        {
            _services = services;
            _context = context;
        }

        /// <summary>
        /// It will manage the service to interact with the client
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns>Deposit status if the transaction is success</returns>
        public async Task<TransactionStatusForDepositAndWithDraw> deposit(TransactionDTO deposit)
        {
            try
            {
                    TransactionStatusForDepositAndWithDraw depositStatus = await _services.depositAsync(deposit);
                    if (depositStatus == null)
                    {
                        return null;
                    }

                    return depositStatus;    
                
            }
            catch (Exception exception)
            {
                _log4net.Info(exception.Message);
                throw;
            }
        }

        public async Task<TransactionStatusForDepositAndWithDraw> withDraw(TransactionDTO amount)
        {
            TransactionStatusForDepositAndWithDraw withDraw = new TransactionStatusForDepositAndWithDraw();
            try
            {
                
                if( AccountBalanceForCheck >= amount.Amount)
                {

                    RulesStatus status = _services.checkRulesForWithDraw(generateAmount(amount.AccountId , AccountBalanceForCheck , amount.Amount));

                    if(status != null)
                    {
                        if(status.Status == "allowed")
                        {
                            withDraw = await _services.withdrawAsync(amount);
                            return withDraw;
                        }
                        else
                        {
                            //throw new MethodAccessException("Transaction Declined");
                            throw new TransferException("Transaction Declined");
                        }
                    }
                }

                return null;
            }
            catch (Exception exception)
            {
                _log4net.Info(exception.Message);
                throw;
                
            }
        }

        public TransactionDTO generateAmount(int accountId , double balance , double amountToWithDraw)
        {

            return new TransactionDTO() { AccountId = accountId, Amount = balance - amountToWithDraw };
        }

        public async Task<TransactionSuccess> transferAmount(TransferDTO transfer)
        {
            try
            {
                if (toCheckIsAccountValid(transfer))
                {
                    TransactionStatusForDepositAndWithDraw withdrawStatus = new TransactionStatusForDepositAndWithDraw();
                    TransactionStatusForDepositAndWithDraw depositStatus = new TransactionStatusForDepositAndWithDraw();
                    TransactionSuccess transactionStatus = new TransactionSuccess();
                    withdrawStatus = await withDraw(new TransactionDTO() { AccountId = transfer.SourceAccountId, Amount = transfer.Amount });
                    if (withdrawStatus != null)
                     {
                            if (withdrawStatus.Message == "Withdraw Successfull")
                            {
                                _log4net.Info("Withdraw from Source Account is successfully completed");

                                depositStatus = await deposit(new TransactionDTO() { AccountId = transfer.DestinationAccountId, Amount = transfer.Amount });

                                if (depositStatus != null)
                                {
                                    return generateTransactionSuccess(depositStatus, withdrawStatus, transfer);
                                }                               
                            }                           
                    }
                    return null;                   
                }
                else
                {
                    throw new AccountNotFound("Invalid Account");
                }
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                throw;
            }
        }

        public Boolean toCheckIsAccountValid(TransferDTO transfer)
        {
            Account sourceAccount = new Account();
            Account destinationAccount = new Account();

            try
            {
                sourceAccount = _services.getAccountBalance(transfer.SourceAccountId);
                destinationAccount = _services.getAccountBalance(transfer.DestinationAccountId);

                if(sourceAccount == null || destinationAccount == null)
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                _log4net.Info(exception.Message);
                throw;
            }

            AccountBalanceForCheck = sourceAccount.AccountBalance;
            return true;
        }

        public int generateCustomerForTransactionHistory(int accountId)
        {
            try
            {
                List<Account> accounts =  _services.getAllaccounts();
                if(accounts != null)
                {
                    foreach (Account account in accounts)
                    {
                        if(account.AccountID == accountId)
                        {
                            return account.CustomerID;
                        }
                    }
                }
                return 0;
            }
            catch (Exception exception)
            {
                _log4net.Info(exception.Message);
                throw;
            }
        }

        public TransactionSuccess generateTransactionSuccess(TransactionStatusForDepositAndWithDraw depositStatus, TransactionStatusForDepositAndWithDraw withdrawStatus, TransferDTO transfer)
        {
            TransactionHistory transactionHistory = new TransactionHistory()
            {
                AccountId = transfer.SourceAccountId,
                CounterpartyAccountId = transfer.DestinationAccountId,
                Amount = transfer.Amount,
                DateofTranasction = DateTime.Today,
                CustomerId = generateCustomerForTransactionHistory(transfer.SourceAccountId)
            };

            if (toSaveTransactionHistory(transactionHistory))
            {
                return new TransactionSuccess() { message = "Transaction Success", DestinationBalance = depositStatus.CurrentBalance, SourceAccountBalance = withdrawStatus.CurrentBalance };
            }
            return null;
        }

        public Boolean toSaveTransactionHistory(TransactionHistory transactionHistory)
        {
            try
            {
                _context.TransactionHistory.Add(transactionHistory);
                _context.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                throw;
            }
        }

        public async Task<List<TransactionHistory>> getTransactionHistory(int customerId)
        {
            try
            {
                return await _context.TransactionHistory.Where(custid => custid.CustomerId == customerId).ToListAsync();
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                throw;
            }


        }
    }
}
