using BookingSystem.Core.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Core.Features.CancelBooking
{
    public class CancelBookingRequest : IRequest<BaseResult>
    {
        public int Id { get; set; }
    }
}
