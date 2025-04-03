using BookingSystem.Core.Common;
using BookingSystem.Core.Features.ImportInventory;
using BookingSystem.Core.Features.ImportMember;
using BookingSystem.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Tests.BookingSystem.WebAPI.Tests.Controllers
{
    public class ImportDataControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ImportDataController _controller;

        public ImportDataControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ImportDataController(_mediatorMock.Object);
        }

        [Fact]
        public async Task ImportMembers_MissingFile_ReturnsBadRequest()
        {
            // Arrange
            var formCollection = new FormCollection(null, null);
            var context = new DefaultHttpContext { Request = { Form = formCollection } };
            _controller.ControllerContext.HttpContext = context;

            // Act
            var result = await _controller.ImportMembers(CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("File is missing.", badRequestResult.Value);
        }

        [Fact]
        public async Task ImportMembers_ValidFile_ReturnsCreated()
        {
            // Arrange
            var fileContent = Encoding.ASCII.GetBytes("name,surname,booking_count,date_joined\r\nSophie,Davis,1,2024-01-02T12:10:11\r\nEmily,Johnson,0,2024-11-12T12:10:12");
            var file = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "data", "members.csv");

            var formCollection = new FormCollection(null, new FormFileCollection { file });
            var context = new DefaultHttpContext { Request = { Form = formCollection } };
            _controller.ControllerContext.HttpContext = context;

            var mediatorResponse = new BaseResult { Succeeded = true };
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<ImportMemberRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            // Act
            var result = await _controller.ImportMembers(CancellationToken.None);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task ImportMembers_InvalidFile_ReturnsBadRequest()
        {
            // Arrange
            var fileContent = Encoding.ASCII.GetBytes("Invalid,Data");
            var file = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "data", "invalid.csv");

            var formCollection = new FormCollection(null, new FormFileCollection { file });            
            var context = new DefaultHttpContext { Request = { Form = formCollection } };
            _controller.ControllerContext.HttpContext = context;

            var mediatorResponse = new BaseResult { Succeeded = false, Message = "Failed to import members." };
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<ImportMemberRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            // Act
            var result = await _controller.ImportMembers(CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to import members.", badRequestResult.Value);
        }

        [Fact]
        public async Task ImportInventory_MissingFile_ReturnsBadRequest()
        {
            // Arrange
            var formCollection = new FormCollection(null, null);
            var context = new DefaultHttpContext { Request = { Form = formCollection } };
            _controller.ControllerContext.HttpContext = context;

            // Act
            var result = await _controller.ImportInventory(CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("File is missing.", badRequestResult.Value);
        }

        [Fact]
        public async Task ImportInventory_ValidFile_ReturnsCreated()
        {
            // Arrange
            var fileContent = Encoding.ASCII.GetBytes("title,description,remaining_count,expiration_date\r\nBali,\"Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus tellus aliquam id. Maecenas non volutpat nisl. Curabitur vestibulum ante non nibh faucibus, sit amet pulvinar turpis finibus\",5,19/11/2030\r\nMadeira,\"Donec condimentum, risus non mollis sollicitudin, est neque sagittis metus, eget aliquam orci quam eget dui. Nam imperdiet, lorem quis lacinia imperdiet, augue libero tincidunt sem, eget pulvinar massa est non metus. Pellentesque et massa nibh.\",4,20/11/2030\r\n");
            var file = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "data", "inventory.csv");

            var formCollection = new FormCollection(null, new FormFileCollection { file });
            var context = new DefaultHttpContext { Request = { Form = formCollection } };
            _controller.ControllerContext.HttpContext = context;

            var mediatorResponse = new BaseResult { Succeeded = true };
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<ImportInventoryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            // Act
            var result = await _controller.ImportInventory(CancellationToken.None);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task ImportInventory_InvalidFile_ReturnsBadRequest()
        {
            // Arrange
            var fileContent = Encoding.ASCII.GetBytes("Invalid,Data");
            var file = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "data", "invalid.csv");

            var formCollection = new FormCollection(null, new FormFileCollection { file });
            var context = new DefaultHttpContext { Request = { Form = formCollection } };
            _controller.ControllerContext.HttpContext = context;

            var mediatorResponse = new BaseResult { Succeeded = false, Message = "Failed to import inventory." };
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<ImportInventoryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            // Act
            var result = await _controller.ImportInventory(CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to import inventory.", badRequestResult.Value);
        }

    }
}
