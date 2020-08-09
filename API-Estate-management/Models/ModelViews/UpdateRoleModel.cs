using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.ModelViews
{
    public class UpdateRoleModel
    {
        [StringLength(256)]
        public string Name { get; set; }
    }
}
