using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Models;
using System.Collections.Generic;

namespace RelayCalculator.Api.Models
{
    public class SwimmerStatModel
    {
        public int SwimmerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int BirthYear { get; set; }

        public Gender Gender { get; set; }

        public CourseTimes ShortCoursePbs { get; set; }

        public CourseTimes LongCoursePbs { get; set; }

        public IEnumerable<Meet> Meets { get; set; }
    }
}
