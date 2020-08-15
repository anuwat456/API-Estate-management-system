using API_Estate_management.Models.Configurations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationRole : IdentityRole
    {
        [Required]
        public bool IsCoreRole { get; set; }

        public virtual ICollection<ApplicationUser> User { get; set; }

        public virtual ICollection<ApplicationRolePermission> Permissions { get; set; }

        public ApplicationRole()
        {
            Permissions = new HashSet<ApplicationRolePermission>();

            Permissions = new List<ApplicationRolePermission>();
        }
    }
}
