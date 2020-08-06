using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
