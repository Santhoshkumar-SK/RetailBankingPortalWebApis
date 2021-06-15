using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Models
{
    public class Statement
    {
        [Key]
        public int StatementID { get; set; }

        [Required]
        [ForeignKey("Account")]
        public int AccountID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string WithdrawalOrDeposit { get; set; }

        [Required]
        public Double ClosingBalance { get; set; }
    }
}
