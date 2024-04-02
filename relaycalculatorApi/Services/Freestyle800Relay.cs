using System;
using System.Collections.Generic;
using System.Linq;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services
{
    public class Freestyle800Relay : IBestTeamCalculationService
    {
        public double GetTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;
            foreach (var position in permutation)
            {
                //TODO: anders oplossen?
                var indTime = course == Course.Long ? swimmers[position].LongCourseTimes.Freestyle200M :
                    swimmers[position].ShortCourseTimes.Freestyle200M;

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
            return swimmers.Select(s => new RelaySwimmer
            {
                ID = s.ID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Age = DateTime.Today.Year - s.BirthYear,
                Time = course == Course.Long ? s.LongCourseTimes.Freestyle200M :
                    s.ShortCourseTimes.Freestyle200M
            }).ToList();
        }
    }
}
