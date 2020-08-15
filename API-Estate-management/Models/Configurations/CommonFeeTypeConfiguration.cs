using API_Estate_management.Models.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Configurations
{
    public class CommonFeeTypeConfiguration : IEntityTypeConfiguration<ApplicationCommonFeeType>
    {
        public void Configure(EntityTypeBuilder<ApplicationCommonFeeType> builder)
        {
            // Primary Key
            builder.HasKey(cft => cft.Id);

            // Auto-increment Primary key
            builder.Property(cft => cft.Id).ValueGeneratedOnAdd();

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties
        }
    }
}
