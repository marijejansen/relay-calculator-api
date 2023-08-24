using System;
using System.Collections.Generic;
using System.Linq;

using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services
{
    public class Backstroke200Relay : IBestTeamCalculationService
    {
        public double GetTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;
            foreach (var position in permutation)
            {
                var indTime = course == Course.Long
                    ? swimmers[position]
                        .LongCourseTimes.Backstroke50M
                    : swimmers[position]
                        .ShortCourseTimes.Backstroke50M;
                if (!(indTime > 0))
                {
                    return 0;
                }

                time += indTime;
            }

            return time;
        }

        public IEnumerable<RelaySwimmer> GetRelaySwimmers(List<Swimmer> swimmers, Course course)
        {
            return swimmers.Select(
                    s => new RelaySwimmer
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Age = DateTime.Today.Year - s.BirthYear,
                        Time = course == Course.Long
                            ? s.LongCourseTimes.Backstroke50M
                            : s.ShortCourseTimes.Backstroke50M
                    })
                .ToList();
        }
    }
}