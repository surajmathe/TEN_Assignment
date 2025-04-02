using BookingSystem.DataAccessLayer.Entity;
using BookingSystem.DataAccessLayer.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Inventory> Inventory { get; set; }

        public DbSet<BookingDetails> BookingDetails { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MemberConfiguration).Assembly);
        }

        public async Task<bool> SaveChangesBooleanAsync(CancellationToken cancellationToken)
        {
            int affectedRecords = await this.SaveChangesAsync(cancellationToken);
            return affectedRecords > 0;
        }

    }
}
