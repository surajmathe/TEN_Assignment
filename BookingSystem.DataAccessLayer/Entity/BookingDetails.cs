using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.Entity
{
    public class BookingDetails
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int InventoryId { get; set; }
        public DateTime BookingTime { get; set; }
        public DateTime? CancelTime { get; set; }
        public bool IsActive { get; set; }

        //Navigation Property
        public Member? Member { get; set; }
        public Inventory? Inventory { get; set; }
    }
}
