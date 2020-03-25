using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RelayCalculator.Services;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Api.Models
{
    public class CalculationRequest
    {
        public IEnumerable<SwimmerModel> Swimmers { get; set; }
        
        public Course Course { get; set; }
        
        public Relay Relay { get; set; }
    }
}
