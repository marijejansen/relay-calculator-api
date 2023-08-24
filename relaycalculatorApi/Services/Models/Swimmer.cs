using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;

namespace RelayCalculator.Services.Models
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
