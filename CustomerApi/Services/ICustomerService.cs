using CustomerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomerApi.Services
{
    public interface ICustomerService
    {

        public  Task<AccountCreationStatus> createAccount(AccountCreationDTO acccreationDTO);

        public HttpResponseMessage createAccountResponse(AccountCreationDTO acccreationDTO);

        public  Boolean registerForAuthentication(int customerId);

        public HttpResponseMessage registerAuthResponse(CustomerAuthRegisterDTO customerAuth);
    }
}
