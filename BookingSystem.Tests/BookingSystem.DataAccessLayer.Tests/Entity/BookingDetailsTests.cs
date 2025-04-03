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
    public class BookingDetailsTests
    {
        private AppDbContext _context;
        public BookingDetailsTests()
        {
            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("BookingSystemTests").Options);
        }

        [Fact]
        public async Task AddingBookingDetails_ReturnsInsertedId()
        {
            //Arrange
            var bookingDetails = new BookingDetails()
            {
                Inventory = new Inventory()
                {
                    Title = "Bali",
                    Description = "Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus",
                    RemainingCount = 5,
                    ExpirationDate = DateTime.Now,
                    IsActive = true
                },
                Member = new Member()
                {
                    FirstName = "Sophie",
                    LastName = "Davis",
                    BookingCount = 1,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                BookingTime = DateTime.Now,
                IsActive = true
            };

            //Act
            await _context.BookingDetails.AddAsync(bookingDetails);
            var result = await _context.SaveChangesBooleanAsync(CancellationToken.None);
            

            //Assert
            Assert.True(result);
            Assert.NotEqual(0, bookingDetails.Id);
            Assert.NotEqual(0, bookingDetails.MemberId);
            Assert.NotEqual(0, bookingDetails.InventoryId);
        }

        [Fact]
        public async Task UpdateBookingDetails_ReturnsUpdatedData()
        {
            //Arrange
            var bookingDetails = new BookingDetails()
            {
                Inventory = new Inventory()
                {
                    Title = "Bali",
                    Description = "Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus",
                    RemainingCount = 5,
                    ExpirationDate = DateTime.Now,
                    IsActive = true
                },
                Member = new Member()
                {
                    FirstName = "Sophie",
                    LastName = "Davis",
                    BookingCount = 1,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                BookingTime = DateTime.Now,
                IsActive = true
            };
            await _context.BookingDetails.AddAsync(bookingDetails);
            await _context.SaveChangesBooleanAsync(CancellationToken.None);

            var dataToBeUpdate = _context.BookingDetails.Find(1);

            //Act
            if(dataToBeUpdate != null)
                dataToBeUpdate.CancelTime = DateTime.Now;
            await _context.SaveChangesBooleanAsync(CancellationToken.None);
            var updatedData = _context.BookingDetails.Find(1);

            //Assert            
            Assert.NotNull(updatedData);
            Assert.NotNull(dataToBeUpdate);
            Assert.NotNull(updatedData.CancelTime);
            Assert.Equal(updatedData.CancelTime, dataToBeUpdate.CancelTime);

        }
    }
}
