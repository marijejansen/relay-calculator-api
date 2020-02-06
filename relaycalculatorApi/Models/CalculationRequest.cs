using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Api.Models
{
    public class CalculationRequest
    {
        public IEnumerable<Swimmer> Swimmers { get; set; }
        public RelayType RelayType { get; set; }
        public Course Course { get; set; }
    }
}
