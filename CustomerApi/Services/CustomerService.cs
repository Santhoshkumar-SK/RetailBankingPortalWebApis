using CustomerApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CustomerApi.Services
{
    public class CustomerService : ICustomerService
    {
        private IConfiguration _config;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CustomerService));


        public CustomerService(IConfiguration config)
        {
            _config = config;
        }


        public  HttpResponseMessage createAccountResponse(AccountCreationDTO acccreationDTO)
        {

            HttpResponseMessage result = new HttpResponseMessage();

            string uriConn = _config.GetValue<string>("ClientConnection:accountApiConn");
            

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriConn + "api/AccountManagement/CreateAccount/");
                _log4net.Info(client.BaseAddress);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    

                    var  response = client.PostAsJsonAsync<AccountCreationDTO>(client.BaseAddress , acccreationDTO);
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

        
        public async Task<AccountCreationStatus> createAccount(AccountCreationDTO acccreationDTO)
        {

            AccountCreationStatus accountCreation = new AccountCreationStatus();

            HttpResponseMessage result = createAccountResponse(acccreationDTO);

            if (result.IsSuccessStatusCode)
            {
                var apiResponse = await result.Content.ReadAsStringAsync();
                accountCreation = JsonConvert.DeserializeObject<AccountCreationStatus>(apiResponse);
                //accountCreation =  result;
                _log4net.Info(result.Content);

            }

            return accountCreation;
        }

        public Boolean registerForAuthentication(int customerId)
        {
            
            CustomerAuthRegisterDTO customerAuth = new CustomerAuthRegisterDTO()
            {
                UserId = customerId,
                Password = "12345",
                Role = "Customer"
            };

            HttpResponseMessage result = registerAuthResponse(customerAuth);

            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }


        public HttpResponseMessage registerAuthResponse(CustomerAuthRegisterDTO customerAuth)
        {
            HttpResponseMessage result = new HttpResponseMessage();

            string uriConn = _config.GetValue<string>("ClientConnection:loginApiConn");


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriConn + "api/Authentication/Register");
                _log4net.Info(client.BaseAddress);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {


                    var response = client.PostAsJsonAsync<CustomerAuthRegisterDTO>(client.BaseAddress, customerAuth);
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

       
    }
}
