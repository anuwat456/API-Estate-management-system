using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(13)]
        public string NumberId { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string AddressLine { get; set; }

        [StringLength(50)]
        public string BirthDate { get; set; }

        [StringLength(100)]
        public string Image { get; set; }

        public ApplicationRole Role { get; set; }
        public string RoleId { get; set; }
    }
}
