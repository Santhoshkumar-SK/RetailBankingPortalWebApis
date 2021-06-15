using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionMicroservice.Exceptions;
using TransactionMicroservice.Models;
using TransactionMicroservice.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TransactionMicroservice.Controllers
{
    
    [Authorize(Roles ="Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TransactionsController));
        ITransactionsRepo _repo;

        public TransactionsController(ITransactionsRepo repo)
        {
            _repo = repo;
        }

        // POST: api/<Transactions/deposit
        /// <summary>
        /// It will deposit the amount in the given account
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns>deposit status</returns>
        [HttpPost]
        [Route("deposit")]
        public async Task<TransactionStatusForDepositAndWithDraw> deposit(TransactionDTO deposit)
        {
            TransactionStatusForDepositAndWithDraw depositStatus = new TransactionStatusForDepositAndWithDraw();
            try
            {
                depositStatus = await _repo.deposit(deposit);

                if (depositStatus == null)
                {
                    return null;
                }

                return depositStatus;
            }
            catch (Exception exception)
            {
                _log4net.Error(exception);
                throw;
            }
        }


        // POST: api/<Transactions/deposit
        /// <summary>
        /// It will withdraw the amount after this account passes the minBalance condition
        /// </summary>
        /// <param name="withDraw"></param>
        /// <returns>deposit status</returns>
        [HttpPost]
        [Route("withdraw")]
        public async Task<TransactionStatusForDepositAndWithDraw> WithDraw([FromBody] TransactionDTO withDraw)
        {
            TransactionStatusForDepositAndWithDraw withDrawStatus = new TransactionStatusForDepositAndWithDraw();
            try
            {
                withDrawStatus = await _repo.withDraw(withDraw);

                if (withDrawStatus == null)
                {
                    return null;
                }

                return withDrawStatus;
            }
            catch (Exception exception)
            {
                _log4net.Error(exception);
                throw;
            }
        }

        // POST: api/<Transactions/transfer
        /// <summary>
        /// It will withdraw the amount after this account passes the minBalance condition
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns>deposit status</returns>
        [HttpPost]
        [Route("transfer")]
        public async Task<ActionResult<TransactionSuccess>> transfer([FromBody] TransferDTO transfer)
        {
            TransactionSuccess transactionStatus = new TransactionSuccess();
            try
            {
                transactionStatus = await _repo.transferAmount(transfer);

                if (transactionStatus == null)
                {
                    return BadRequest("Transaction Declined");
                }

                return Ok(transactionStatus);
            }
            catch(AccountNotFound exception)
            {
                _log4net.Error(exception);
                return BadRequest("Account Not Found");
            }
            catch (TransferException exception)
            {
                _log4net.Error(exception);
                return BadRequest("Transaction Declined because of Insufficent Balance");
            }
        }

        //Post : api/Transactions/getTransactions
        [HttpPost]
        [Route("getTransactions")]
        public async Task<ActionResult<List<TransactionHistory>>> transactionHistory([FromQuery] int customerId)
        {
            try
            {
                List<TransactionHistory> transactionHistory = await _repo.getTransactionHistory(customerId);
                
                if(transactionHistory.Count == 0)
                {
                    return BadRequest();
                }

                return transactionHistory;
            }
            catch (Exception exception)
            {
                _log4net.Info(exception);
                return BadRequest();
            }
        }

    }
}
