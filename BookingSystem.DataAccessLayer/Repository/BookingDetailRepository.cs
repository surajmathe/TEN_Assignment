using BookingSystem.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DataAccessLayer.Repository
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        private readonly AppDbContext _context;
        public BookingDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddBookingDetails(BookingDetails booking, CancellationToken cancellationToken)
        {
            await _context.BookingDetails.AddAsync(booking, cancellationToken);
            return await _context.SaveChangesBooleanAsync(cancellationToken);
        }

        public async Task<BookingDetails?> GetBookingDetailsById(int bookingId, CancellationToken cancellationToken)
        {
            return await _context.BookingDetails
                .Include(prop => prop.Inventory)
                .Include(prop => prop.Member)
                .FirstOrDefaultAsync(prop => prop.Id == bookingId, cancellationToken);
        }

        public async Task<bool> UpdateBookingDetails(BookingDetails booking, CancellationToken cancellationToken)
        {
            _context.BookingDetails.Update(booking);
            return await _context.SaveChangesBooleanAsync(cancellationToken);
        }
    }
}
