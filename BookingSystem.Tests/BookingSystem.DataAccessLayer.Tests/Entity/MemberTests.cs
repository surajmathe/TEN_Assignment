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
    public class MemberTests
    {
        private AppDbContext _context;
        public MemberTests()
        {
            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("BookingSystemTests").Options);
        }

        [Fact]
        public async Task AddingMember_ReturnsInsertedId()
        {
            //Arrange
            var member = new Member()
            {
                FirstName = "Sophie",
                LastName = "Davis",
                BookingCount = 1,
                CreatedDate = DateTime.Now,
            };

            //Act
            await _context.Members.AddAsync(member);
            await _context.SaveChangesBooleanAsync(CancellationToken.None);

            //Assert
            Assert.True(member.IsActive);
            Assert.NotEqual(0, member.Id);
            Assert.Null(member.BookingDetails);
        }

        [Fact]
        public async Task UpdateMember_ReturnsUpdatedData()
        {
            //Arrange
            var member = new Member()
            {
                FirstName = "Sophie",
                LastName = "Davis",
                BookingCount = 1,
                CreatedDate = DateTime.Now,
            };
            await _context.Members.AddAsync(member);
            await _context.SaveChangesBooleanAsync(CancellationToken.None);

            //Act
            var dataToUpdate = _context.Members.Find(1);
            if(dataToUpdate != null)
            {
                dataToUpdate.BookingCount = 3;
            }
            await _context.SaveChangesBooleanAsync(CancellationToken.None);
            var updatedData = _context.Members.Find(1);

            //Assert
            Assert.True(updatedData != null);
            Assert.Equal(3, updatedData.BookingCount);
        }
    }
}
