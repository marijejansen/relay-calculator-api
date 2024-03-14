using System;
using System.Collections.Generic;
using RelayCalculator.Api.Services.Enums;

namespace RelayCalculator.Api.Models
{
    public class SwimmerMeetResult
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public Gender Gender { get; set; }
        public Course Course { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<EventResult> EventResults { get; set; }

    }

    public class EventResult
    {
        public SwimEvent SwimEvent { get; set; }
        public double Time { get; set; }
    }
}
