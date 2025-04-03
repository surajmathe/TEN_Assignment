using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.Entity
{
    public class Member
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int BookingCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        //Navigation Property
        public List<BookingDetails>? BookingDetails { get; set; }

        public Member()
        {
            IsActive = true;
        }
    }
}
