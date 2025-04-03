using BookingSystem.Core.Features.BookItem;
using BookingSystem.Core.Features.CancelBooking;
using BookingSystem.WebAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingSystem.WebAPI.Controllers
{
    [ApiController]
    public class BookController : ControllerBase
    {
        public IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("~/api/book")]
        [HttpPost]        
        public async Task<IActionResult> BookItem([FromBody]BookItemRequestParameters request, CancellationToken cancellationToken)
        {

            BookItemRequest bookRequest = new()
            {
                InventoryId = request.InventoryId,
                MemberId = request.MemberId,
            };

            var result = await _mediator.Send(bookRequest, cancellationToken);

            if (result.Succeeded) {
                return Created();
            }
            else
            {                
                return BadRequest(result.Message);
            }
        }

        [Route("~/api/cancel/{bookingId}")]
        [HttpPatch]
        public async Task<IActionResult> CancelBooking([FromRoute] int bookingId, CancellationToken cancellationToken)
        {
            CancelBookingRequest cancelBookingRequest = new()
            {
                Id = bookingId,
            };

            var result = await _mediator.Send(cancelBookingRequest, cancellationToken);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
