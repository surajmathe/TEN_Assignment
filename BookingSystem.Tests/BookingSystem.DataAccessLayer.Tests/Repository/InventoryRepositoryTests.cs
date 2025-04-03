using BookingSystem.DataAccessLayer;
using BookingSystem.DataAccessLayer.Entity;
using BookingSystem.DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Tests.BookingSystem.DataAccessLayer.Tests.Repository
{
    public class InventoryRepositoryTests
    {
        private IInventoryRepository _inventoryRepository;

        public InventoryRepositoryTests()
        {
            _inventoryRepository = new InventoryRepository(
                    new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("BookingSystemTests").Options)
                );
        }

        [Fact]
        public async Task AddBulkInventory_AddBulkInventory_ReturnsTrue()
        {
            //Arrange 
            List<Inventory> inventories = new()
            {
                new()
                {
                    Title = "Bali",
                    Description = "Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus",
                    RemainingCount = 5,
                    ExpirationDate = DateTime.Now,
                }
            };

            //Act
            var result = await this._inventoryRepository.AddBulkInventory(inventories, CancellationToken.None);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetInventoryById_GetExistingData_ReturnsData()
        {
            //Arrange
            await AddBulkInventory_AddBulkInventory_ReturnsTrue();
            int inventoryId = 1;

            //Act
            var result = await _inventoryRepository.GetInventoryById(inventoryId, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(inventoryId, result.Id);
        }

        [Fact]
        public async Task GetInventoryById_GetNonExistingData_ReturnsNull()
        {
            //Arrange
            int inventoryId = 0;

            //Act
            var result = await _inventoryRepository.GetInventoryById(inventoryId, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateInventory_UpdateData_ReturnsTrue()
        {
            //Arrange
            await AddBulkInventory_AddBulkInventory_ReturnsTrue();
            int inventoryId = 1;

            Inventory? dataToBeUpdated = await _inventoryRepository.GetInventoryById(inventoryId, CancellationToken.None);

            if(dataToBeUpdated != null)
            {
                dataToBeUpdated.RemainingCount = 3;
                //Act
                var result = await _inventoryRepository.UpdateInventory(dataToBeUpdated, CancellationToken.None);

                //Assert
                Assert.True(result);
            }            
        }
    }
}
