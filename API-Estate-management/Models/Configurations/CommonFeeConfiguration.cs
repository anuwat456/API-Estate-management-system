using API_Estate_management.Models.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Configurations
{
    public class CommonFeeConfiguration : IEntityTypeConfiguration<ApplicationCommonFee>
    {
        public void Configure(EntityTypeBuilder<ApplicationCommonFee> builder)
        {
            // Primary Key
            builder.HasKey(cf => cf.Id);

            // Auto-increment Primary key
            builder.Property(cf => cf.Id).ValueGeneratedOnAdd();

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties
            builder.HasOne(cf => cf.House)
                .WithMany(h => h.CommonFees)
                .HasForeignKey(cf => cf.HouseId);

            builder.HasOne(cf => cf.CommonFeeType)
                .WithMany(cft => cft.CommonFees)
                .HasForeignKey(cf => cf.CommonFeeTypeId);
        }
    }
}
