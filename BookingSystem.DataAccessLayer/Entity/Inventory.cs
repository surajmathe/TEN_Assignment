using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.Entity
{
    public class Inventory
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int RemainingCount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        //Navigation Property
        public List<BookingDetails>? BookingDetails { get; set; }

        public Inventory()
        {
            IsActive = true;
        }
    }
}
