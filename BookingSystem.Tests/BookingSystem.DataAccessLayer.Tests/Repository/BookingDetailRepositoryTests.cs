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
    public class BookingDetailRepositoryTests
    {
        private IBookingDetailRepository _bookingDetailRepository;
        public BookingDetailRepositoryTests()
        {
            _bookingDetailRepository = new BookingDetailRepository(
                new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("BookingSystemTests").Options)
                );
        }

        [Fact]
        public async Task AddBookingDetails_AddNewBookingDetails_ReturnsInsertedId()
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
            var result = await _bookingDetailRepository.AddBookingDetails(bookingDetails, CancellationToken.None);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetBookingDetailsById_GetExisingData_ReturnsData()
        {
            //Arrange
            await AddBookingDetails_AddNewBookingDetails_ReturnsInsertedId();
            var bookingId = 1;

            //Act
            var result = await _bookingDetailRepository.GetBookingDetailsById(bookingId, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetBookingDetailsById_GetNonExistingData_ReturnsNull()
        {
            //Arrange
            var bookingId = 0;

            //Act
            var result = await _bookingDetailRepository.GetBookingDetailsById(bookingId, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateBookingDetails_UpdateBooking_ReturnsTrue()
        {
            //Arrange
            await AddBookingDetails_AddNewBookingDetails_ReturnsInsertedId();
            var bookingId = 1;
            var dataToBeUpdate = await _bookingDetailRepository.GetBookingDetailsById(bookingId, CancellationToken.None);
            if(dataToBeUpdate != null)
            {
                dataToBeUpdate.IsActive = false;

                //Act
                var result = await _bookingDetailRepository.UpdateBookingDetails(dataToBeUpdate, CancellationToken.None);

                //Assert
                Assert.True(result);
            }
        }
    }
}
