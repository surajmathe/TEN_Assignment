using BookingSystem.Core.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Core.Features.ImportInventory
{
    public class ImportInventoryRequest : IRequest<BaseResult>
    {
        public required Stream File { get; set; }
    }
}
