using CustomerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Repository
{
    public interface ICustomerRepo
    {
        public  Task<CustomerCreationStatus> createCustomer(Customer customerDetails);
        public Customer getCustomerDetails(int Customer_Id);
    }
}
