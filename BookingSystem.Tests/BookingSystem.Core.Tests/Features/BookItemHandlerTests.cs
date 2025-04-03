using BookingSystem.Core.Constants;
using BookingSystem.Core.Features.BookItem;
using BookingSystem.DataAccessLayer.Entity;
using BookingSystem.DataAccessLayer.Repository;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Tests.BookingSystem.Core.Tests.Features
{
    public class BookItemHandlerTests
    {
        private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;
        private readonly Mock<IMemberRepository> _memberRepositoryMock;
        private readonly Mock<IBookingDetailRepository> _bookingDetailRepositoryMock;
        private readonly BookItemHandler _handler;

        public BookItemHandlerTests()
        {
            _inventoryRepositoryMock = new Mock<IInventoryRepository>();
            _memberRepositoryMock = new Mock<IMemberRepository>();
            _bookingDetailRepositoryMock = new Mock<IBookingDetailRepository>();
            _handler = new BookItemHandler(_inventoryRepositoryMock.Object, _memberRepositoryMock.Object, _bookingDetailRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_MemberNotFound_ReturnsError()
        {
            // Arrange
            var request = new BookItemRequest { MemberId = 1, InventoryId = 1 };
            _memberRepositoryMock
                .Setup(repo => repo.GetMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Member?)null); 

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Member not found.", result.Message);
        }

        [Fact]
        public async Task Handle_MemberInactive_ReturnsError()
        {
            // Arrange
            var request = new BookItemRequest { MemberId = 1, InventoryId = 1 };
            var inactiveMember = new Member { Id = 1, IsActive = false };
            _memberRepositoryMock
                .Setup(repo => repo.GetMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(inactiveMember);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Member not found.", result.Message);
        }

        [Fact]
        public async Task Handle_MemberReachedMaxBookings_ReturnsError()
        {
            // Arrange
            var request = new BookItemRequest { MemberId = 1, InventoryId = 1 };
            var member = new Member { Id = 1, IsActive = true, BookingCount = AppConstants.MAX_BOOKINGS };
            _memberRepositoryMock
                .Setup(repo => repo.GetMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(member);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal($"Member has already reached max booking count i.e. {AppConstants.MAX_BOOKINGS}", result.Message);
        }

        [Fact]
        public async Task Handle_InventoryNotFound_ReturnsError()
        {
            // Arrange
            var request = new BookItemRequest { MemberId = 1, InventoryId = 1 };
            var activeMember = new Member { Id = 1, IsActive = true };
            _memberRepositoryMock
                .Setup(repo => repo.GetMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(activeMember);

            _inventoryRepositoryMock
                .Setup(repo => repo.GetInventoryById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Inventory?)null); // Simulate inventory not found

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Item not found.", result.Message);
        }

        [Fact]
        public async Task Handle_InventoryInactive_ReturnsError()
        {
            // Arrange
            var request = new BookItemRequest { MemberId = 1, InventoryId = 1 };
            var activeMember = new Member { Id = 1, IsActive = true };
            _memberRepositoryMock
                .Setup(repo => repo.GetMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(activeMember);

            var inactiveInventory = new Inventory { Id = 1, IsActive = false };
            _inventoryRepositoryMock
                .Setup(repo => repo.GetInventoryById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(inactiveInventory);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Item not found.", result.Message);
        }

        [Fact]
        public async Task Handle_ValidRequest_CreatesBooking_ReturnsSucceededTrue()
        {
            // Arrange
            var request = new BookItemRequest { MemberId = 1, InventoryId = 1 };
            var activeMember = new Member { Id = 1, IsActive = true, BookingCount = 0 };
            var availableInventory = new Inventory { Id = 1, IsActive = true, RemainingCount = 5, ExpirationDate = DateTime.UtcNow.AddDays(1) };

            _memberRepositoryMock
                .Setup(repo => repo.GetMemberById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(activeMember);

            _inventoryRepositoryMock
                .Setup(repo => repo.GetInventoryById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(availableInventory);

            _inventoryRepositoryMock
                .Setup(repo => repo.UpdateInventory(It.IsAny<Inventory>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));

            _memberRepositoryMock
                .Setup(repo => repo.UpdateMember(It.IsAny<Member>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));

            _bookingDetailRepositoryMock
                .Setup(repo => repo.AddBookingDetails(It.IsAny<BookingDetails>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Message);

            // Verify methods were called
            _inventoryRepositoryMock.Verify(repo => repo.UpdateInventory(It.IsAny<Inventory>(), It.IsAny<CancellationToken>()), Times.Once);
            _memberRepositoryMock.Verify(repo => repo.UpdateMember(It.IsAny<Member>(), It.IsAny<CancellationToken>()), Times.Once);
            _bookingDetailRepositoryMock.Verify(repo => repo.AddBookingDetails(It.IsAny<BookingDetails>(), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
