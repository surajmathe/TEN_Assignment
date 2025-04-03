using BookingSystem.Core.Features.CancelBooking;
using BookingSystem.DataAccessLayer.Entity;
using BookingSystem.DataAccessLayer.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Tests.BookingSystem.Core.Tests.Features
{
    public class CancelBookingHandlerTests
    {
        private readonly Mock<IBookingDetailRepository> _bookingDetailRepositoryMock;
        private readonly CancelBookingHandler _handler;

        public CancelBookingHandlerTests()
        {
            _bookingDetailRepositoryMock = new Mock<IBookingDetailRepository>();
            _handler = new CancelBookingHandler(_bookingDetailRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_BookingNotFound_ReturnsError()
        {
            // Arrange
            var request = new CancelBookingRequest { Id = 1 };
            _bookingDetailRepositoryMock
                .Setup(repo => repo.GetBookingDetailsById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((BookingDetails?)null); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Booking not found.", result.Message);
        }

        [Fact]
        public async Task Handle_BookingAlreadyCancelled_ReturnsError()
        {
            // Arrange
            var request = new CancelBookingRequest { Id = 1 };
            var cancelledBooking = new BookingDetails { Id = 1, IsActive = false };
            _bookingDetailRepositoryMock
                .Setup(repo => repo.GetBookingDetailsById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cancelledBooking); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Booking is already cancelled.", result.Message);
        }

        [Fact]
        public async Task Handle_ValidBooking_CancelsSuccessfully()
        {
            // Arrange
            var request = new CancelBookingRequest { Id = 1 };
            var activeBooking = new BookingDetails
            {
                Id = 1,
                IsActive = true,
                Member = new Member { Id = 1, BookingCount = 5 },
                Inventory = new Inventory { Id = 1, RemainingCount = 10 }
            };
            _bookingDetailRepositoryMock
                .Setup(repo => repo.GetBookingDetailsById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(activeBooking);

            _bookingDetailRepositoryMock
                .Setup(repo => repo.UpdateBookingDetails(It.IsAny<BookingDetails>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Message);

            // Verify the booking was updated
            Assert.False(activeBooking.IsActive);
            Assert.NotNull(activeBooking.CancelTime);
            Assert.Equal(4, activeBooking.Member.BookingCount); 
            Assert.Equal(11, activeBooking.Inventory.RemainingCount); 

            _bookingDetailRepositoryMock.Verify(repo => repo.UpdateBookingDetails(It.IsAny<BookingDetails>(), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
