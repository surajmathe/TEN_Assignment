using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.DataAccessLayer.EntityConfigurations
{
    internal class BookingDetailConfiguration : IEntityTypeConfiguration<BookingDetails>
    {
        public void Configure(EntityTypeBuilder<BookingDetails> builder)
        {
            builder
                .HasOne(prop => prop.Member)
                .WithMany(prop => prop.BookingDetails)
                .HasForeignKey(prop => prop.MemberId);

            builder
                .HasOne(prop => prop.Inventory)
                .WithMany(prop => prop.BookingDetails)
                .HasForeignKey(prop => prop.InventoryId);
        }
    }
}
