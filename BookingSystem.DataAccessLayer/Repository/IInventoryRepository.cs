using BookingSystem.DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.Repository
{
    public interface IInventoryRepository
    {
        Task<bool> AddBulkInventory(List<Inventory> members, CancellationToken cancellationToken);

        Task<Inventory?> GetInventoryById(int id, CancellationToken cancellationToken);

        Task<bool> UpdateInventory(Inventory inventory, CancellationToken cancellationToken);
    }
}
