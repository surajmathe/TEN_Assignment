using BookingSystem.DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;
        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddBulkInventory(List<Inventory> members, CancellationToken cancellationToken)
        {
            await _context.Inventory.AddRangeAsync(members, cancellationToken);
            return await _context.SaveChangesBooleanAsync(cancellationToken);
        }

        public async Task<Inventory?> GetInventoryById(int id, CancellationToken cancellationToken)
        {
            return await _context.Inventory.FindAsync(id, cancellationToken);
        }

        public async Task<bool> UpdateInventory(Inventory inventory, CancellationToken cancellationToken)
        {
            _context.Inventory.Update(inventory);
            return await _context.SaveChangesBooleanAsync(cancellationToken);
        }
    }
}
