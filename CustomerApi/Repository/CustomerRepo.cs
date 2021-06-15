using CustomerApi.Data;
using CustomerApi.Models;
using CustomerApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Repository
{
    public class CustomerRepo : ICustomerRepo
    {

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CustomerRepo));

        CustomerContext _context;
        ICustomerService _service;
        public CustomerRepo(ICustomerService service, CustomerContext context)
        {
            _service = service;
            _context = context;
        }
        
        public async Task<CustomerCreationStatus> createCustomer(Customer customerDetails)
        {
            CustomerCreationStatus creationStatus = null;
            try
            {
                _context.customers.Add(customerDetails);
                _context.SaveChanges();
                _log4net.Info("Customer Created Successfully");
                int accountId = await createAccountForCustomer(customerDetails);
                int registerForLogin = registerForAuthentication(customerDetails.customerId);
                if(accountId != -1 && registerForLogin == 1)
                {
                    creationStatus = customerCreationStatus(customerDetails.customerId, "Account Created Successfully", accountId);
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                return null;
            }
            return creationStatus;
        }
        public CustomerCreationStatus customerCreationStatus(int customerId, string message, int accountId)
        {
            return new CustomerCreationStatus()
            {
                CustomerId = customerId,
                Message = message,
                AccountId = accountId
            };
        }

        public Customer getCustomerDetails(int customerId)
        {
            return _context.customers.Where(cusId => cusId.customerId == customerId).FirstOrDefault();
        }

        public async Task<int> createAccountForCustomer(Customer customer)
        {
            AccountCreationStatus accountCreation = new AccountCreationStatus();

            try
            {
                AccountCreationDTO account = new AccountCreationDTO() { CustomerId = customer.customerId, CustomerAccountType = customer.customerAccountType };
                accountCreation = await _service.createAccount(account);


                if(accountCreation == null)
                {
                    return -1;
                }
                _log4net.Info("Account Created Successfully");
                return accountCreation.AccountID;
            }
            catch ( Exception exception)
            {
                _log4net.Error(exception.Message);
                return -1;
            }

        }

        public  int registerForAuthentication(int customerId)
        {
            try
            {
                Boolean  result = _service.registerForAuthentication(customerId);

                if (result)
                    return 1;

            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                return -1;
            }

            return -1;
        }
    }
}
