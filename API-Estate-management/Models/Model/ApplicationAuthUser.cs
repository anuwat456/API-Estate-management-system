using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationAuthUser
    {
        [StringLength(256), Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(256), Required]
        public string UserName { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
