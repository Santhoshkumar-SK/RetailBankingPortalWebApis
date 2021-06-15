using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models
{
    public class Customer
    {
        [Key]
        public int customerId { get; set; }
        [Required]

        public string customerName { get; set; }
        [Required]

        public string customerAddress { get; set; }
        [Required]

        public DateTime customerDOB { get; set; }
        [Required]

        public string customerPannumber { get; set; }
        [Required]
        public string customerAdhaarnumber { get; set; }
        [Required]
        public string customerAccountType { get; set; }


    }

}