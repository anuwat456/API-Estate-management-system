using API_Estate_management.Models.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Configurations
{
    public class MaintenanceConfiguration : IEntityTypeConfiguration<ApplicationMaintenance>
    {
        public void Configure(EntityTypeBuilder<ApplicationMaintenance> builder)
        {
            // Primary Key
            builder.HasKey(m => m.Id);

            // Auto-increment Primary key
            builder.Property(m => m.Id).ValueGeneratedOnAdd();

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties
            builder.HasOne(m => m.House)
                .WithMany(h => h.Maintenances)
                .HasForeignKey(m => m.HouseId);
        }
    }
}
