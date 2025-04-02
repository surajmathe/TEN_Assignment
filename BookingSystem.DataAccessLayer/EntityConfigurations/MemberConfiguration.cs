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
    internal class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder
                .Property(prop => prop.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(prop => prop.LastName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
