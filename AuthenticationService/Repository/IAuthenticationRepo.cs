using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Repository
{
    public interface IAuthenticationRepo
    {
        public Task<Boolean> authenticateUser(UserDTO user);

        public string generateJSONWebToken(UserDTO userInfo);

        public Task<Boolean> authenticateCustomer(UserDTO user);

        public bool authenticateEmployee(UserDTO user);

        public Task<Boolean> addCustomer(UserDTO user);
    }
}
