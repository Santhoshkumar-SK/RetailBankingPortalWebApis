using AuthenticationService.Models;
using AuthenticationService.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthenticationController));

        IAuthenticationRepo _repo;

        public AuthenticationController(IAuthenticationRepo repo)
        {
            _repo = repo;
        }
        // GET api/<AuthenticationController>/5
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> login([FromBody] UserDTO user)
        {

            var result = await _repo.authenticateUser(user);

            if (result)
            {

                string tokenString = _repo.generateJSONWebToken(user);
                if (tokenString != null)
                    return Ok(tokenString);
                else
                    return BadRequest("Problem in Generating Token");
            }
                

            return BadRequest("Authentication Failed");
        }

        // POST api/<AuthenticationController>
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> addCustomer([FromBody] UserDTO user)
        {
            var result = await _repo.addCustomer(user);

            if(result)
            {
                return Ok("Customer Added");
            }

            return BadRequest("Customer not added");
        }

        
    }
}
