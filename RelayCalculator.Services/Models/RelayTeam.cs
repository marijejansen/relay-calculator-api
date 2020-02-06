using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services.Models
{
    public class RelayTeam
    {
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public IEnumerable<RelaySwimmer> Swimmers { get; set; }
        public Double Time { get; set; }
    }
}
