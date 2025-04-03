using BookingSystem.DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace BookingSystem.Core.CsvMappings
{
    public class CsvInventoryMapping: CsvMapping<Inventory>
    {
        public CsvInventoryMapping() : base()
        {
            MapProperty(0, prop => prop.Title);
            MapProperty(1, prop => prop.Description);
            MapProperty(2, prop => prop.RemainingCount);
            MapProperty(3, prop => prop.ExpirationDate, new DateTimeConverter("dd/MM/yyyy"));            
        }
    }
}
