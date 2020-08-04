using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Create Roles for or application

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationRole>().HasData
            (
                new { Id = "1", Name = "Admin", NormalizationName = "ADMIN"},
                new { Id = "2", Name = "Manager", NormalizationName = "MANAGER" },
                new { Id = "3", Name = "Guest", NormalizationName = "GUEST" }
            );
        }
    }
}
