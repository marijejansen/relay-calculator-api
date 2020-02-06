using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Api.Models
{
    public class SwimmerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public Gender Gender { get; set; }
        public CourseTimes ShortCourseTimes { get; set; }
        public CourseTimes LongCourseTimes { get; set; }
    }
}
