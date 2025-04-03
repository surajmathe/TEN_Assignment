using BookingSystem.DataAccessLayer;
using BookingSystem.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Tests.BookingSystem.DataAccessLayer.Tests.Entity
{
    public class InventoryTests
    {
        private AppDbContext _context;
        public InventoryTests()
        {
            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("BookingSystemTests").Options);
        }

        [Fact]
        public async Task AddingInventory_ReturnsInsertedId()
        {
            //Arrange
            var inventory = new Inventory()
            {
                Title = "Bali",
                Description = "Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus",
                RemainingCount = 5,
                ExpirationDate = DateTime.Now,
            };

            //Act
            await _context.Inventory.AddAsync(inventory);
            await _context.SaveChangesBooleanAsync(CancellationToken.None);

            //Assert            
            Assert.True(inventory.IsActive);
            Assert.NotEqual(0, inventory.Id);
            Assert.Null(inventory.BookingDetails);
        }

        [Fact]
        public async Task UpdateInventory_ReturnsUpdatedData()
        {
            //Arrange
            var inventory = new Inventory()
            {
                Title = "Bali",
                Description = "Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus",
                RemainingCount = 5,
                ExpirationDate = DateTime.Now,
            };
            await _context.Inventory.AddAsync(inventory);
            await _context.SaveChangesBooleanAsync(CancellationToken.None);

            //Act
            var dataToUpdate = _context.Inventory.Find(1);
            if (dataToUpdate != null)
            {
                dataToUpdate.RemainingCount = 3;
            }
            await _context.SaveChangesBooleanAsync(CancellationToken.None);
            var updatedData = _context.Inventory.Find(1);

            //Assert            
            Assert.True(updatedData != null);
            Assert.Equal(3, updatedData.RemainingCount);
        }
    }
}
