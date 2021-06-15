using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models
{
    public class CustomerCreationStatus
    {
        public string Message { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }


    }
}
