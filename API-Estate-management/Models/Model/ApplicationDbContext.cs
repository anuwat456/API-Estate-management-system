using API_Estate_management.Models.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationRolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<ApplicationRolePermission>().HasIndex(rp => rp.PermissionId);

            modelBuilder.Entity<ApplicationRolePermission>()
                .HasOne<ApplicationRole>(rp => rp.Role)
                .WithMany(r => r.Permissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<ApplicationRolePermission>()
                .HasOne<ApplicationPermission>(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
        }

        // Entities
        public DbSet<ApplicationPermission> Permissions { get; set; }
        public DbSet<ApplicationRolePermission> RolePermissions { get; set; }
        public DbSet<ApplicationNews> News { get; set; }
        public DbSet<ApplicationHouse> Houses { get; set; }
        public DbSet<ApplicationHouseType> HouseTypes { get; set; }
        public DbSet<ApplicationMaintenance> Maintenances { get; set; }
        public DbSet<ApplicationCommonFee> CommonFees { get; set; }
        public DbSet<ApplicationCommonFeeType> CommonFeeTypes { get; set; }



        // Create Roles for or application
        /*
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
        */
    }
}
