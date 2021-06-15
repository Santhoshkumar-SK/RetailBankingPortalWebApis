using AccountManagementApi.Enum;
using AccountManagementApi.Models;
using AccountManagementApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountManagementController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AccountManagementController));

        
        IAccountManagementRepo _repo;
        

        public AccountManagementController(IAccountManagementRepo repo)
        {

            _repo = repo;
           
        }


        //POST: api/AccountManagement/CreateAccount
        [HttpPost]
        [Route("CreateAccount")]
        public async Task<ActionResult<AccountCreationStatus>> createAccount([FromBody] AccountCreationDTO accountCreationDTO )
        {
            _log4net.Info("Account Creation Services Called..");
            
            var accountCreationStatus =await _repo.createAccountForCustomer(createDataForAccount(accountCreationDTO.CustomerId , accountCreationDTO.CustomerAccountType));

            if(accountCreationStatus == null)
            {
                return BadRequest(accountCreationStatus);
            }

            return Ok(accountCreationStatus);
        }

        //GET : api/AccountManagement/getCustomerAccounts
        [HttpGet]
        [Route("getCustomerAccounts")]
        public async Task<ActionResult<List<AccountDetailsForSending>>> getCustomerAccounts(int customerId)
        {
            _log4net.Info("Get Customer Accounts Service called..");

            var customerAccounts =await  _repo.getCustomerAccounts(customerId);

            if(customerAccounts.Count == 0)
            {
                return BadRequest("No Accounts Found");
            }

            return Ok(customerAccounts);
        }

        //GET : api/AccountManagement/getCustomerAccounts
        [HttpGet]
        [Route("getAccount")]
        public async Task<ActionResult<List<AccountDetailsForSending>>> getAccount([FromQuery]int accountId)
        {
            _log4net.Info("Get Accounts Details Service called..");

            var accountDetail = await _repo.getAccount(accountId);

            if (accountDetail == null)
            {
                return BadRequest("Account Not Found");
            }

            return Ok(accountDetail);
        }

        //GET : api/AccountManagement/getAccountStatement
        [HttpGet]
        [Route("getAccountStatement")]
        public async  Task<ActionResult<List<Statement>>> getAccountStatement([FromQuery] GetStatementDTO getStatement )
        {
            _log4net.Info("Get Account Statement Service called..");

            List<Statement> statements = null;

            List<DateTime> dates = generateDateForStatement(getStatement);

            if (dates == null)
            {
                return BadRequest("Invalid Date Range");
            }

            statements = await _repo.getAccountStatement(getStatement.AccountId, dates[0] , dates[1]);
            
            if (statements == null )
            {
                return BadRequest("No Account found");
            }
            else if(statements.Count == 0)
            {
                return BadRequest("No Statement found");
            }

            return Ok(statements);
        }


        //POST : api/AccountManagement/deposit
        [HttpPost]
        [Route("deposit")]
        [Authorize]
        public async Task<ActionResult<TransactionStatusForDepositAndWithDraw>> deposit([FromBody] TransactionDTO transactionDTO)
        {
            _log4net.Info("Deposit Service called..");

            var depositStatus = await _repo.deposit(transactionDTO);

            if (depositStatus == null)
            {
                return BadRequest("Account Not Found");
            }

            return Ok(depositStatus);
        }


        //POST : api/AccountManagement/withdraw
        [HttpPost]
        [Route("withdraw")]
        [Authorize]
        public async Task<ActionResult<TransactionStatusForDepositAndWithDraw>> withdraw([FromBody] TransactionDTO transactionDTO)
        {
            _log4net.Info("Withdraw Service called..");

            var withDrawStatus = await _repo.withdraw(transactionDTO);

            if(withDrawStatus == null)
            {
                return BadRequest("Account Not Found");
            }

            return Ok(withDrawStatus);
        }


        public static Account createDataForAccount(int customerId, string customerAccountType)
        {
            return new Account() { CustomerID = customerId, AccountBalance = 5000, AccountType = customerAccountType };
        }


        //GET : api/AccountManagement/getAllCustomerAccounts
        [HttpGet]
        [Route("getAllCustomerAccounts")]
        public async Task<ActionResult<List<Account>>> getAllAccounts()
        {
            _log4net.Info("Getting All Customer Accounts");

            List<Account> customerAccounts = await _repo.getAllCustomerAccounts();

            if(customerAccounts.Count == 0)
            {
                return BadRequest(customerAccounts);
            }

            return Ok(customerAccounts);
        }

        public static List<DateTime> generateDateForStatement(GetStatementDTO getStatement)
        {
            //List<DateTime> dateTimes = new List<DateTime>();
            ForgettingDates _date = new ForgettingDates();

            if (getStatement.FromDate.HasValue && getStatement.ToDate.HasValue)
            {
                if (((DateTime)getStatement.FromDate) > ((DateTime)getStatement.ToDate))
                {
                    return null;
                }
                else if (((DateTime)getStatement.FromDate).Month == _date.currentDate().Month && ((DateTime)getStatement.FromDate).Day == _date.currentDate().Day && ((DateTime)getStatement.FromDate).Year == _date.currentDate().Year)
                {
                    return null;
                }
                else if (((DateTime)getStatement.ToDate).Month == _date.currentDate().Month && ((DateTime)getStatement.ToDate).Day == _date.currentDate().Day && ((DateTime)getStatement.ToDate).Year == _date.currentDate().Year)
                {
                    return new List<DateTime>() { (DateTime)getStatement.FromDate, ((DateTime)getStatement.ToDate).AddDays(-1) };
                }
                else
                {

                    return new List<DateTime>() { (DateTime)getStatement.FromDate, ((DateTime)getStatement.ToDate) };
                }
            }
            else
            {
                List<DateTime> dates = new List<DateTime>() { _date.startDatefortheStatement(), _date.endDatefortheStatement() };
                return dates;
            }

        }
    }
}
