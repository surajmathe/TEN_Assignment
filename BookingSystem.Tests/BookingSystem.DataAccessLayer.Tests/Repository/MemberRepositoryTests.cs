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
    public class MemberRepositoryTests
    {
        private IMemberRepository _memberRepository;
        public MemberRepositoryTests()
        {
            _memberRepository = new MemberRepository(
                    new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("BookingSystemTests").Options)
                );
        }

        [Fact]
        public async Task AddBulkMembers_AddMembers_ReturnsTrue()
        {
            //Arrange
            List<Member> members = new()
            {
                new()
                {
                    FirstName = "Sophie",
                    LastName = "Davis",
                    BookingCount = 1,
                    CreatedDate = DateTime.Now,
                }
            };

            //Act
            var result = await _memberRepository.AddBulkMembers(members, CancellationToken.None);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetMemberById_GetExisingData_ReturnsData()
        {
            //Arrange
            await AddBulkMembers_AddMembers_ReturnsTrue();
            int memberId = 1;

            //Act
            var result = await _memberRepository.GetMemberById(memberId, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, memberId);
        }

        [Fact]
        public async Task GetMemberById_GetNonExisingData_ReturnsNull()
        {
            //Arrange
            int memberId = 0;

            //Act
            var result = await _memberRepository.GetMemberById(memberId, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Fact] 
        public async Task UpdateMember_UpdateExistingData_ReturnsTrue()
        {
            //Arrange
            await AddBulkMembers_AddMembers_ReturnsTrue();
            var memberId = 1;
            Member? dataToBeUpdate = await _memberRepository.GetMemberById(memberId, CancellationToken.None);

            if(dataToBeUpdate != null)
            {
                dataToBeUpdate.BookingCount = 3;

                //Act
                var result = await _memberRepository.UpdateMember(dataToBeUpdate, CancellationToken.None);

                //Assert
                Assert.True(result);
            }            
        }
    }
}
