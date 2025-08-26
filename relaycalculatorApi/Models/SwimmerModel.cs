using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Models
{
    public class SwimmerModel
    { 
        public int ID { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int BirthYear { get; set; }

        public Gender Gender { get; set; }

        public CourseTimes? ShortCourseTimes { get; set; }

        public CourseTimes? LongCourseTimes { get; set; }
    }
}
