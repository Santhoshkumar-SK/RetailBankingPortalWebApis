using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Models
{
    public class AccountCreationDTO
    {
        public int CustomerId { get; set; }

        public string CustomerAccountType { get; set; }
    }
}
