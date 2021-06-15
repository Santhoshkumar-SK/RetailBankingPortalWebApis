using CustomerApi.Models;
using CustomerApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    [Authorize(Roles = "Employee")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CustomerController));
        ICustomerRepo _repo;

       public CustomerController(ICustomerRepo repo)
        {
            _repo = repo;
        }


        [HttpGet]
        [Route("getCustomerDetails")]
        public IActionResult getCustomerDetails(int Customer_Id)
        {
          _log4net.Info("get customer details is called with id:" + Customer_Id);

            var getCustomerDetails = _repo.getCustomerDetails(Customer_Id);

            if (getCustomerDetails == null)
            {
                return BadRequest("Could not fild the Customer");
            }

            return Ok(getCustomerDetails);
        }


        
        [HttpPost]
        [Route("createCustomer")]
        public async Task<ActionResult<CustomerCreationStatus>> createCustomer([FromBody] Customer customer)
        {
            _log4net.Info("Creation of customer is initiated");

            var createCustomer = await _repo.createCustomer(customer);
            if(createCustomer == null)
            {
                return BadRequest("Customer not created");
            }
            return Ok(createCustomer);
        }

    }
}
