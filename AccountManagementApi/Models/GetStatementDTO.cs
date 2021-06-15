using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AccountManagementApi.Models
{
    public class GetStatementDTO
    {
        public int AccountId { get; set; }


        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
