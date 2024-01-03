using RelayCalculator.Api.Services.Enums;
using System;
using System.Collections.Generic;

namespace RelayCalculator.Api.Models
{
    public class Meet
    {
        public int MeetId { get; set; }
        public Course Course { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<RacePerformance> RacePerformances { get; set; }
    }
}
