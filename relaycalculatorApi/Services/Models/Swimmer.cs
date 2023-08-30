using RelayCalculator.Api.Services.Enums;

namespace RelayCalculator.Api.Services.Models
{
    public class Swimmer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public Gender Gender { get; set; }
        public string ClubName { get; set; }
        public CourseTimes ShortCourseTimes { get; set; }
        public CourseTimes LongCourseTimes { get; set; }
    }
}
