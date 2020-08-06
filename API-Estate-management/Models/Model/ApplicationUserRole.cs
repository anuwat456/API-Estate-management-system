using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public const string Admin = "Admin";

        public const string Manager = "Manager";

        public const string Guest = "Guest";
    }
}
