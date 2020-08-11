using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationRolePermission
    {
        public ApplicationRole Role { get; set; }
        public string RoleId { get; set; }

        public ApplicationPermission Permission { get; set; }
        public string PermissionId { get; set; }
    }
}
