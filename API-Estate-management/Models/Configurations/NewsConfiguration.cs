using API_Estate_management.Models.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Estate_management.Models.Configurations
{
    public class NewsConfiguration : IEntityTypeConfiguration<ApplicationNews>
    {
        public void Configure(EntityTypeBuilder<ApplicationNews> builder)
        {
            // Primary Key
            builder.HasKey(n => n.Id);

            // Auto-increment Primary key
            builder.Property(n => n.Id).ValueGeneratedOnAdd();

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties
            builder.HasOne(n => n.User)
                .WithMany(u => u.News)
                .HasForeignKey(u => u.UserId);
        }
    }
}
