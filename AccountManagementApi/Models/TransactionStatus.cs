using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Models
{
    public class TransactionStatus
    {
        [Key]
        public int TransactionStatusID { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string SourceBalance { get; set; }

        [Required]
        public string DestinationBalance { get; set; }
    }


    public class TransactionStatusForDepositAndWithDraw
    {
        public string Message { get; set; }

        public Double CurrentBalance { get; set; }
    }
}
