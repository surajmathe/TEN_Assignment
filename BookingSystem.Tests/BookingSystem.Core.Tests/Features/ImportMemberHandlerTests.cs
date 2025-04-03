using BookingSystem.Core.Common;
using BookingSystem.Core.Features.ImportMember;
using BookingSystem.DataAccessLayer.Entity;
using BookingSystem.DataAccessLayer.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingSystem.Tests.BookingSystem.Core.Tests.Features
{
    public class ImportMemberHandlerTests
    {
        private readonly Mock<IMemberRepository> _memberRepositoryMock;
        private readonly ImportMemberHandler _handler;

        public ImportMemberHandlerTests()
        {
            _memberRepositoryMock = new Mock<IMemberRepository>();
            _handler = new ImportMemberHandler(_memberRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_NoValidRecordsFound_ReturnsError()
        {
            // Arrange
            var request = new ImportMemberRequest
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
        public async Task Handle_ValidCSV_ImportsSuccessfully_VerifiesMemberCount()
        {
            // Arrange
            var request = new ImportMemberRequest
            {
                File = new MemoryStream(Encoding.ASCII.GetBytes("name,surname,booking_count,date_joined\r\nSophie,Davis,1,2024-01-02T12:10:11\r\nEmily,Johnson,0,2024-11-12T12:10:12"))
            };

            _memberRepositoryMock
                .Setup(repo => repo.AddBulkMembers(It.IsAny<List<Member>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            BaseResult result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Message);

            // Verify members count passed to the repository
            _memberRepositoryMock.Verify(repo =>
                repo.AddBulkMembers(It.Is<List<Member>>(list => list.Count == 2), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
