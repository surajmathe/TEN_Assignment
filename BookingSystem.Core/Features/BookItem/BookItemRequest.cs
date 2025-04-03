using BookingSystem.Core.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Core.Features.BookItem
{
    public class BookItemRequest : IRequest<BaseResult>
    {
        public int MemberId { get; set; }
        public int InventoryId { get; set; }
    }
}
