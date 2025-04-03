using BookingSystem.Core.Common;
using BookingSystem.Core.Constants;
using BookingSystem.DataAccessLayer.Entity;
using BookingSystem.DataAccessLayer.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Core.Features.BookItem
{
    public class BookItemHandler : IRequestHandler<BookItemRequest, BaseResult>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IBookingDetailRepository _bookDetailRepository;
        public BookItemHandler(IInventoryRepository inventoryRepository, IMemberRepository memberRepository, IBookingDetailRepository bookDetailRepository)
        {
            _inventoryRepository = inventoryRepository;
            _memberRepository = memberRepository;
            _bookDetailRepository = bookDetailRepository;
        }

        public async Task<BaseResult> Handle(BookItemRequest request, CancellationToken cancellationToken)
        {
            BaseResult result = new();

            var member = await _memberRepository.GetMemberById(request.MemberId, cancellationToken);
            if (member == null || !member.IsActive)
            {
                result.Message = "Member not found.";
                return result;
            }
            else if (member.BookingCount >= AppConstants.MAX_BOOKINGS)
            {
                result.Message = $"Member has already reached max booking count i.e. {AppConstants.MAX_BOOKINGS}";
                return result;
            }

            var inventory = await _inventoryRepository.GetInventoryById(request.InventoryId, cancellationToken);
            if (inventory == null || !inventory.IsActive)
            {
                result.Message = "Item not found.";
                return result;
            }
            else if (inventory.RemainingCount <= 0)
            {
                result.Message = "Item is not available in the inventory.";
                return result;
            }
            else if (inventory.ExpirationDate < DateTime.Now)
            {
                result.Message = "Inventory is expired.";
                return result;
            }


            BookingDetails booking = new()
            {
                InventoryId = request.InventoryId,
                MemberId = request.MemberId,
                BookingTime = DateTime.Now,
                IsActive = true
            };
            member.BookingCount += 1;
            inventory.RemainingCount -= 1;
            await _inventoryRepository.UpdateInventory(inventory, cancellationToken);
            await _memberRepository.UpdateMember(member, cancellationToken);

            result.Succeeded = await _bookDetailRepository.AddBookingDetails(booking, cancellationToken);

            return result;
        }
    }
}
