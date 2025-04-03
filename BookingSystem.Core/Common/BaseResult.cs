using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Core.Common
{
    public class BaseResult
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
    }
}
