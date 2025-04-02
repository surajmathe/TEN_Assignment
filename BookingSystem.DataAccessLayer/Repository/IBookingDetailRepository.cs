using BookingSystem.DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.Repository
{
    public interface IBookingDetailRepository
    {
        Task<bool> AddBookingDetails(BookingDetails booking, CancellationToken cancellationToken);

        Task<bool> UpdateBookingDetails(BookingDetails booking, CancellationToken cancellationToken);

        Task<BookingDetails?> GetBookingDetailsById(int bookingId,  CancellationToken cancellationToken);
    }
}
