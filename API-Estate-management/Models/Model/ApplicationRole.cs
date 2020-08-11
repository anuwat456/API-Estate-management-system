using API_Estate_management.Models.Configurations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUser> User { get; set; }
        public virtual ICollection<ApplicationRolePermission> RolePermissions { get; set; }

        public ApplicationRole()
        {
            RolePermissions = new HashSet<ApplicationRolePermission>();
        }
    }
}
