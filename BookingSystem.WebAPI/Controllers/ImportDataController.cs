using BookingSystem.Core.Features.ImportInventory;
using BookingSystem.Core.Features.ImportMember;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ImportDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("Member")]
        [HttpPost]
        public async Task<ActionResult> ImportMembers(CancellationToken cancellationToken)
        {

            if (this.HttpContext.Request.Form == null || this.HttpContext.Request.Form.Files == null || this.HttpContext.Request.Form.Files.Count == 0)
            {
                return BadRequest("File is missing.");
            }
            using Stream fileStream = new MemoryStream();
            await this.HttpContext.Request.Form.Files[0].CopyToAsync(fileStream, cancellationToken);
            fileStream.Position = 0;
            ImportMemberRequest request = new() { File = fileStream };
            var result = await _mediator.Send(request, cancellationToken);

            if (result.Succeeded)
            {
                return Created();
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [Route("Inventory")]
        [HttpPost]
        public async Task<ActionResult> ImportInventory(CancellationToken cancellationToken)
        {
            if (this.HttpContext.Request.Form == null || this.HttpContext.Request.Form.Files == null || this.HttpContext.Request.Form.Files.Count == 0)
            {
                return BadRequest("File is missing.");
            }
            using Stream fileStream = new MemoryStream();
            await this.HttpContext.Request.Form.Files[0].CopyToAsync(fileStream, cancellationToken);
            fileStream.Position = 0;
            ImportInventoryRequest request = new() { File = fileStream };
            var result = await _mediator.Send(request, cancellationToken);

            if (result.Succeeded)
            {
                return Created();
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
