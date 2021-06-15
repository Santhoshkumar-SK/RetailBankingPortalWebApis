using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string  Password { get; set; }

        public string Role { get; set; }
    }
}
