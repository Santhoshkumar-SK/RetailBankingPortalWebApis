using Microsoft.Extensions.Configuration;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TransactionMicroservice.Models;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace TransactionMicroservice.Services
{
    public class TransactionsService : ITransactionsService
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TransactionsService));
        IConfiguration _config;
        public TransactionsService(IConfiguration config)
        {
            _config = config;
        }

        

        /// <summary>
        /// It will deposit the amount from the account with postasync in accountmanagement api
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns>transaction status with success message</returns>
        public async Task<TransactionStatusForDepositAndWithDraw> depositAsync(TransactionDTO deposit)
        {
            TransactionStatusForDepositAndWithDraw depositStatus = new TransactionStatusForDepositAndWithDraw();

            HttpResponseMessage result = depositResponse(deposit);

            if (result.IsSuccessStatusCode)
            {
                var apiResponse = await result.Content.ReadAsStringAsync();
                depositStatus = JsonConvert.DeserializeObject<TransactionStatusForDepositAndWithDraw>(apiResponse);
                _log4net.Info("Deposit Successfull");
                return depositStatus;
            }

            return null;
        }

        public HttpResponseMessage depositResponse(TransactionDTO deposit)
        {
            HttpResponseMessage result = new HttpResponseMessage();

            string uriConn = _config.GetValue<string>("ClientConnection:accountApiConn");


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriConn + "api/AccountManagement/deposit");
                _log4net.Info(client.BaseAddress);
                var token = generateJSONWebToken();
                _log4net.Info(token);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = client.PostAsJsonAsync<TransactionDTO>(client.BaseAddress, deposit);
                    response.Wait();
                    result = response.Result;
                }
                catch (Exception e)
                {
                    _log4net.Error("Exception Occured" + e);
                    return null;
                }
            }
            return result;
        }

        /// <summary>
        /// It will check the bussiness rule if the minmum balance is maintained or not 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>transaction status with success message</returns>
        public RulesStatus checkRulesForWithDraw(TransactionDTO amount)
        {
            RulesStatus rulesStatus = new RulesStatus();

            HttpResponseMessage response = getRulesResponse(amount);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                rulesStatus = JsonConvert.DeserializeObject<RulesStatus>(result);
                _log4net.Info("Successfully runned the rules check");
                return rulesStatus;
            }

            _log4net.Info("Rules check build failed");
            return rulesStatus;
        }

        public Account getAccountBalance(int accountId)
        {
            Account account = new Account();

            HttpResponseMessage response = getAccountBalanceResponse(accountId);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                account = JsonConvert.DeserializeObject<Account>(result);
                _log4net.Info("Getted Account" + account.AccountID);
                return account;
            }

            return null;
        }


        public HttpResponseMessage getAccountBalanceResponse(int accountId)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_config.GetValue<string>("ClientConnection:accountApiConn")); //+ "api/AccountManagement/getAccount";
                string addresstosend = "api/AccountManagement/getAccount?accountId=" + accountId;
                HttpResponseMessage response = client.GetAsync(addresstosend).Result;
                _log4net.Info("Get account response created");
                return response;
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                throw exception;
            }
        }

        public HttpResponseMessage getRulesResponse(TransactionDTO amount)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_config.GetValue<string>("ClientConnection:rulesApiConn"));
                string addresstosend = "api/Rules/EvaluateMinBal?AccountId="+amount.AccountId +"&Amount="+amount.Amount;
                HttpResponseMessage response = client.GetAsync(addresstosend).Result;
                _log4net.Info("Get rules response created");
                return response;
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                throw exception;
            }
        }

        public async Task<TransactionStatusForDepositAndWithDraw> withdrawAsync(TransactionDTO withdraw)
        {
            TransactionStatusForDepositAndWithDraw withDrawnStatus = new TransactionStatusForDepositAndWithDraw();

            HttpResponseMessage result = withDrawnResponse(withdraw);

            if (result.IsSuccessStatusCode)
            {
                var apiResponse = await result.Content.ReadAsStringAsync();
                withDrawnStatus = JsonConvert.DeserializeObject<TransactionStatusForDepositAndWithDraw>(apiResponse);
                _log4net.Info("WithDrawn Successfull");
                return withDrawnStatus;
            }

            return null;
        }

        public HttpResponseMessage withDrawnResponse(TransactionDTO withDraw)
        {
            HttpResponseMessage result = new HttpResponseMessage();

            string uriConn = _config.GetValue<string>("ClientConnection:accountApiConn");


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriConn + "api/AccountManagement/withdraw");
                _log4net.Info(client.BaseAddress);
                string token = generateJSONWebToken();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = client.PostAsJsonAsync<TransactionDTO>(client.BaseAddress, withDraw);
                    response.Wait();
                    result = response.Result;
                }
                catch (Exception e)
                {
                    _log4net.Error("Exception Occured" + e);
                    return null;
                }
            }
            return result;
        }

        public List<Account> getAllaccounts()
        {
            List<Account> account = new List<Account>();

            HttpResponseMessage response = getAllaccountseResponse();

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                account = JsonConvert.DeserializeObject<List<Account>>(result);
                _log4net.Info("Getted All Account");
                return account;
            }

            return null;
        }

        public HttpResponseMessage getAllaccountseResponse()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_config.GetValue<string>("ClientConnection:accountApiConn")); 
                string addresstosend = "api/AccountManagement/getAllCustomerAccounts";
                HttpResponseMessage response = client.GetAsync(addresstosend).Result;
                _log4net.Info("Get all account response created");
                return response;
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                throw exception;
            }
        }

       
        public string generateJSONWebToken()
        {
            _log4net.Info("Generating Token Started..");
            string token = null;

            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                var tokenDescripter = new SecurityTokenDescriptor
                {
                    
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = credentials
                };

                var tokenHandeler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandeler.CreateToken(tokenDescripter);
                token = tokenHandeler.WriteToken(securityToken);
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                return null;
            }

            return token;

        }
    }
}
