using BookingSystem.Core.Common;
using BookingSystem.Core.Features.ImportInventory;
using BookingSystem.DataAccessLayer.Entity;
using BookingSystem.DataAccessLayer.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace BookingSystem.Tests.BookingSystem.Core.Tests.Features
{
    public class ImportInventoryHandlerTests
    {
        private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;
        private readonly ImportInventoryHandler _handler;

        public ImportInventoryHandlerTests()
        {
            _inventoryRepositoryMock = new Mock<IInventoryRepository>();
            _handler = new ImportInventoryHandler(_inventoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_NoValidRecordsFound_ReturnsError()
        {
            // Arrange
            var request = new ImportInventoryRequest
            {
                File = new MemoryStream(Encoding.ASCII.GetBytes("Invalid,Data"))
            };

            // Act
            BaseResult result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("No valid records found in the CSV", result.Message);
        }

        [Fact]
        public async Task Handle_ValidCSV_ImportsSuccessfully()
        {
            // Arrange
            var request = new ImportInventoryRequest
            {
                File = new MemoryStream(Encoding.ASCII.GetBytes("title,description,remaining_count,expiration_date\r\nBali,\"Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus tellus aliquam id. Maecenas non volutpat nisl. Curabitur vestibulum ante non nibh faucibus, sit amet pulvinar turpis finibus\",5,19/11/2030\r\nMadeira,\"Donec condimentum, risus non mollis sollicitudin, est neque sagittis metus, eget aliquam orci quam eget dui. Nam imperdiet, lorem quis lacinia imperdiet, augue libero tincidunt sem, eget pulvinar massa est non metus. Pellentesque et massa nibh.\",4,20/11/2030\r\n"))
            };            

            _inventoryRepositoryMock
                .Setup(repo => repo.AddBulkInventory(It.IsAny<List<Inventory>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            BaseResult result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Message);

            // Verify that AddBulkInventory was called once and 2 valid item should be parsed
            _inventoryRepositoryMock.Verify(repo => repo.AddBulkInventory(It.Is<List<Inventory>>((lst) => lst.Count == 2), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
