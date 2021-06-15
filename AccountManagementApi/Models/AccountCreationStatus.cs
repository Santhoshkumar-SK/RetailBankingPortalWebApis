using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Models
{
    public class AccountCreationStatus
    {
        [Key]
        public int MessageID { get; set; }
        [Required]
        [ForeignKey("Account")]
        public int AccountID { get; set; }
        public Account Account { get; set; }

        public string Message { get; set; }
    }
}
