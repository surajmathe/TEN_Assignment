using BookingSystem.Core.Common;
using BookingSystem.Core.CsvMappings;
using BookingSystem.DataAccessLayer.Entity;
using BookingSystem.DataAccessLayer.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace BookingSystem.Core.Features.ImportInventory
{
    public class ImportInventoryHandler : IRequestHandler<ImportInventoryRequest, BaseResult>
    {

        private readonly IInventoryRepository _inventoryRepository;
        public ImportInventoryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<BaseResult> Handle(ImportInventoryRequest request, CancellationToken cancellationToken)
        {
            BaseResult result = new();
            List<Inventory> inventories = new();
            try
            {
                CsvParser<Inventory> csvParser = new(new CsvParserOptions(true, ','), new CsvInventoryMapping());
                var parsingResult = csvParser
                    .ReadFromStream(request.File, Encoding.ASCII)
                    .ToList();
                if (parsingResult != null && parsingResult.Count > 0)
                {                    
                    parsingResult.ForEach(item =>
                    {                        
                        if(item.IsValid) 
                            inventories.Add(item.Result);
                    });
                }
            }
            catch
            {
                result.Message = "Failed to parse the CSV file.";
                return result;
            }

            if(inventories.Count == 0)
            {
                result.Message = "No valid records found in the CSV";
                return result;
            }

            result.Succeeded = await this._inventoryRepository.AddBulkInventory(inventories, cancellationToken);

            return result;
        }
    }
}
