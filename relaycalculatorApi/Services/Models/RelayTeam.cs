using System;
using System.Collections.Generic;
using RelayCalculator.Api.Services.Enums;

namespace RelayCalculator.Api.Services.Models
{
    public class RelayTeam
    {
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public int AgeGroup { get; set; }
        public IEnumerable<RelaySwimmer> Swimmers { get; set; }
        public Double Time { get; set; }
    }
}
