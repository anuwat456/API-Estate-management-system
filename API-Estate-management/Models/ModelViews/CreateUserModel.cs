using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.ModelViews
{
    public class CreateUserModel
    {
        [StringLength(13), Required]
        public string NumberId { get; set; }

        [StringLength(50), Required]
        public string FullName { get; set; }

        [StringLength(256), Required]
        public string UserName { get; set; }

        [StringLength(256), Required]
        public string Email { get; set; }

        [StringLength(256), Required]
        public string Password { get; set; }

        [StringLength(256), Required]
        public string PhoneNumber { get; set; }

        [StringLength(50), Required]
        public string BirthDate { get; set; }

        [StringLength(100), Required]
        public string AddressLine { get; set; }

        [StringLength(100)]
        public string Image { get; set; }
    }
}
