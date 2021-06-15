using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public Double AccountBalance { get; set; }
    }
}
