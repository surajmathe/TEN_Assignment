using BookingSystem.Core.Common;
using BookingSystem.Core.Features.BookItem;
using BookingSystem.Core.Features.CancelBooking;
using BookingSystem.WebAPI.Controllers;
using BookingSystem.WebAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Tests.BookingSystem.WebAPI.Tests.Controllers
{
    public class BookControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BookController(_mediatorMock.Object);
        }

        [Fact]
        public async Task BookItem_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var requestParameters = new BookItemRequestParameters
            {
                InventoryId = 1,
                MemberId = 2
            };
            var mediatorResponse = new BaseResult
            {
                Succeeded = true
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<BookItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            // Act
            var result = await _controller.BookItem(requestParameters, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task BookItem_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var requestParameters = new BookItemRequestParameters
            {
                InventoryId = 1,
                MemberId = 2
            };
            var mediatorResponse = new BaseResult
            {
                Succeeded = false,
                Message = "Failed to book item"
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<BookItemRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            // Act
            var result = await _controller.BookItem(requestParameters, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to book item", badRequestResult.Value);
        }

        [Fact]
        public async Task CancelBooking_ValidRequest_ReturnsOk()
        {
            // Arrange
            int bookingId = 1;
            var mediatorResponse = new BaseResult
            {
                Succeeded = true
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CancelBookingRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            // Act
            var result = await _controller.CancelBooking(bookingId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CancelBooking_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            int bookingId = 1;
            var mediatorResponse = new BaseResult
            {
                Succeeded = false,
                Message = "Failed to cancel booking"
            };

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<CancelBookingRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            // Act
            var result = await _controller.CancelBooking(bookingId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to cancel booking", badRequestResult.Value);
        }

    }
}
