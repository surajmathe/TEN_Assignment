using BookingSystem.Core.Common;
using BookingSystem.DataAccessLayer.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Core.Features.CancelBooking
{
    public class CancelBookingHandler : IRequestHandler<CancelBookingRequest, BaseResult>
    {
        private readonly IBookingDetailRepository _bookDetailRepository;
        public CancelBookingHandler(IBookingDetailRepository bookDetailRepository)
        {
            _bookDetailRepository = bookDetailRepository;
        }

        public async Task<BaseResult> Handle(CancelBookingRequest request, CancellationToken cancellationToken)
        {
            BaseResult result = new();

            var bookingDetail = await _bookDetailRepository.GetBookingDetailsById(request.Id, cancellationToken);

            if (bookingDetail == null)
            {
                result.Message = "Booking not found.";
                return result;
            }
            else if (!bookingDetail.IsActive)
            {
                result.Message = "Booking is already cancelled.";
                return result;
            }

            bookingDetail.IsActive = false;
            bookingDetail.CancelTime = DateTime.Now;

            if (bookingDetail.Member != null)
                bookingDetail.Member.BookingCount -= 1;

            if (bookingDetail.Inventory != null)
                bookingDetail.Inventory.RemainingCount += 1;

            result.Succeeded = await _bookDetailRepository.UpdateBookingDetails(bookingDetail, cancellationToken);

            return result;
        }
    }
}
