using BookingSystem.DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.Repository
{
    public interface IMemberRepository
    {
        Task<bool> AddBulkMembers(List<Member> members, CancellationToken cancellationToken);

        Task<Member?> GetMemberById(int memberId, CancellationToken cancellationToken);

        Task<bool> UpdateMember(Member member, CancellationToken cancellationToken);
    }
}
