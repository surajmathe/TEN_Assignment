using System.ComponentModel.DataAnnotations;

namespace BookingSystem.WebAPI.Models
{
    public class BookItemRequestParameters
    {
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int InventoryId { get; set; }
    }
}
