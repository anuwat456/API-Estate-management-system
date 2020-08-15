using API_Estate_management.Models.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Configurations
{
    public class HouseTypeConfiguration : IEntityTypeConfiguration<ApplicationHouseType>
    {
        public void Configure(EntityTypeBuilder<ApplicationHouseType> builder)
        {
            // Primary Key
            builder.HasKey(ht => ht.Id);

            // Auto-increment Primary key
            builder.Property(n => n.Id).ValueGeneratedOnAdd();

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties
        }
    }
}
