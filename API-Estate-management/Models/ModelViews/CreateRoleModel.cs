﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.ModelViews
{
    public class CreateRoleModel
    {
        [StringLength(256), Required]
        public string Name { get; set; }

        [Required]
        public bool IsCoreRole { get; set; }
    }
}
