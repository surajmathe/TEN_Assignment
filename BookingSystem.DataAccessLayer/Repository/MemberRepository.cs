using BookingSystem.DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;
        public MemberRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> AddBulkMembers(List<Member> members, CancellationToken cancellationToken)
        {            
            await _context.Members.AddRangeAsync(members, cancellationToken);
            return await _context.SaveChangesBooleanAsync(cancellationToken);
        }

        public async Task<Member?> GetMemberById(int memberId, CancellationToken cancellationToken)
        {
            return await _context.Members.FindAsync(memberId, cancellationToken);
        }

        public async Task<bool> UpdateMember(Member member, CancellationToken cancellationToken)
        {
            _context.Members.Update(member);
            return await _context.SaveChangesBooleanAsync(cancellationToken);
        }
    }
}
