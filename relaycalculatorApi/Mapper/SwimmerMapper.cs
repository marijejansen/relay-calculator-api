using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Models;
using Swimmer = RelayCalculator.Api.Services.Models.Swimmer;

namespace RelayCalculator.Api.Mapper
{
    public class SwimmerMapper : ISwimmerMapper
    {
        public Swimmer Map(SwimmerModel swimmer)
        {
            return new Swimmer
            {
                BirthYear = swimmer.BirthYear,
                FirstName = swimmer.FirstName,
                LastName = swimmer.LastName,
                Gender = swimmer.Gender,
                ShortCourseTimes = swimmer.ShortCourseTimes != null ? new CourseTimes()
                {
                    Freestyle50M = swimmer.ShortCourseTimes.Freestyle50M,
                    Freestyle100M = swimmer.ShortCourseTimes.Freestyle100M,
                    Freestyle200M = swimmer.ShortCourseTimes.Freestyle200M,
                    Backstroke50M = swimmer.ShortCourseTimes.Backstroke50M,
                    Backstroke100M = swimmer.ShortCourseTimes.Backstroke100M,
                    Breaststroke50M = swimmer.ShortCourseTimes.Breaststroke50M,
                    Breaststroke100M = swimmer.ShortCourseTimes.Breaststroke100M,
                    Butterfly50M = swimmer.ShortCourseTimes.Butterfly50M,
                    Butterfly100M = swimmer.ShortCourseTimes.Butterfly100M
                } : new CourseTimes(),
                LongCourseTimes = swimmer.LongCourseTimes != null ? new CourseTimes()
                {
                    Freestyle50M = swimmer.LongCourseTimes.Freestyle50M,
                    Freestyle100M = swimmer.LongCourseTimes.Freestyle100M,
                    Freestyle200M = swimmer.LongCourseTimes.Freestyle200M,
                    Backstroke50M = swimmer.LongCourseTimes.Backstroke50M,
                    Backstroke100M = swimmer.LongCourseTimes.Backstroke100M,
                    Breaststroke50M = swimmer.LongCourseTimes.Breaststroke50M,
                    Breaststroke100M = swimmer.LongCourseTimes.Breaststroke100M,
                    Butterfly50M = swimmer.LongCourseTimes.Butterfly50M,
                    Butterfly100M = swimmer.LongCourseTimes.Butterfly100M
                } : new CourseTimes()
            };
        }

        public SwimmerModel ReverseMap(Swimmer swimmer)
        {
            return new SwimmerModel
            {
                BirthYear = swimmer.BirthYear,
                FirstName = swimmer.FirstName,
                LastName = swimmer.LastName,
                Gender = swimmer.Gender,
                ShortCourseTimes = swimmer.ShortCourseTimes,
                LongCourseTimes = swimmer.LongCourseTimes
            };
        }
    }
}
