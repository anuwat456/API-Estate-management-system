using API_Estate_management.Models.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationPermission
    {
        [Key]
        public int Id { get; set; }

        [StringLength(256), Required]
        public string Name { get; set; }

        [StringLength(256), Required]
        public string Level { get; set; }

        public int ParentId { get; set; }        // Will ParentId reference from Id 

        [StringLength(256), Required]
        public string Title { get; set; }

        public virtual ICollection<ApplicationRolePermission> RolePermissions { get; set; } 

        public ApplicationPermission()
        {
            RolePermissions = new HashSet<ApplicationRolePermission>();
        }
    }
}
