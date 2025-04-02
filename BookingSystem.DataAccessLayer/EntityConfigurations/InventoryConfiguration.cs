using BookingSystem.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.EntityConfigurations
{
    internal class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder
                .Property(prop => prop.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(prop => prop.Description)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
