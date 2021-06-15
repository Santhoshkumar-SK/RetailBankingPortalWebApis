using System;

namespace RulesMicroService.Models
{
    public class Account
    {

        public int AccountID { get; set; }


        public int CustomerID { get; set; }


        public string AccountType { get; set; }


        public Double AccountBalance { get; set; }
    }
}
