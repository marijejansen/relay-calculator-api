using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services
{
    class Freestyle400Relay : IBestTeamCalculationService
    {
        public double GetTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;
            foreach (var position in permutation)
            {
                var indTime = course == Course.Long ? swimmers[position].LongCourseTimes.Freestyle100M :
                    swimmers[position].ShortCourseTimes.Freestyle100M;
                if (!(indTime > 0))
                {
                    return 0;
                }

                time += indTime;
            }

            return time;
        }
        public IEnumerable<RelaySwimmer> GetRelaySwimmersByPermutation(int[] permutation, List<Swimmer> swimmers, Course course)
        {
        return permutation.Select(n => swimmers[n]).Select(s => new RelaySwimmer
        {
            FirstName = s.FirstName,
            LastName = s.LastName,
            Age = DateTime.Today.Year - s.BirthYear,
            Time = course == Course.Long ? s.LongCourseTimes.Freestyle100M :
                s.ShortCourseTimes.Freestyle100M
        }).ToList();
        }
    }
}
