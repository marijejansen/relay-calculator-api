using System;
using System.Collections.Generic;
using System.Linq;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services
{
    public class Breaststroke200Relay : IBestTeamCalculationService
    {
        public double GetTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;
            foreach (var position in permutation)
            {
                var indTime = course == Course.Long
                    ? swimmers[position]?
                        .LongCourseTimes?.Breaststroke50M
                    : swimmers[position]?
                        .ShortCourseTimes?.Breaststroke50M;
                if (!(indTime > 0) || indTime == null)
                {
                    return 0;
                }

                time += (double)indTime;
            }

            return time;
        }

        public IEnumerable<RelaySwimmer> GetRelaySwimmers(List<Swimmer> swimmers, Course course)
        {
            return swimmers.Select(
                    s => new RelaySwimmer
                    {
                        ID = s.ID,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Age = DateTime.Today.Year - s.BirthYear,
                        Time = course == Course.Long
                            ? s.LongCourseTimes?.Breaststroke50M
                            : s.ShortCourseTimes?.Breaststroke50M
                    })
                .ToList();
        }
    }
}