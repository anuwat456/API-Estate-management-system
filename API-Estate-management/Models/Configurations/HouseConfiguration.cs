using API_Estate_management.Models.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Configurations
{
    public class HouseConfiguration : IEntityTypeConfiguration<ApplicationHouse>
    {
        public void Configure(EntityTypeBuilder<ApplicationHouse> builder)
        {
            // Primary Key
            builder.HasKey(h => h.Id);

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties
            builder.HasOne(h => h.User)
                .WithMany(u => u.Houses)
                .HasForeignKey(h => h.UserId);

            builder.HasOne(h => h.HouseType)
                .WithMany(ht => ht.Houses)
                .HasForeignKey(h => h.HouseTypeId);
        }
    }
}
