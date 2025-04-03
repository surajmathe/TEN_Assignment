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
    public class CsvMemberMapping : CsvMapping<Member>
    {
        public CsvMemberMapping() : base()
        {
            MapProperty(0, prop => prop.FirstName);
            MapProperty(1, prop => prop.LastName);
            MapProperty(2, prop => prop.BookingCount);
            MapProperty(3, prop => prop.CreatedDate, new DateTimeConverter("yyyy-MM-ddTHH:mm:ss"));
        }
    }
}
