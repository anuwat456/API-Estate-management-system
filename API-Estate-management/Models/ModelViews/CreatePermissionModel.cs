using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.ModelViews
{
    public class CreatePermissionModel
    {
        [StringLength(256), Required]
        public string Name { get; set; }

        [StringLength(256), Required]
        public string Level { get; set; }

        public string ParentId { get; set; }        // Will ParentId reference from Id

        [StringLength(256), Required]
        public string Title { get; set; }
    }
}
