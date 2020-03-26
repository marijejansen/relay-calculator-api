using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services
{
    public class Medley200Relay : IBestTeamCalculationService
    {
        public double GetTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;
            for (int i = 0; i < permutation.Length; i++)
            {
                var number = permutation[i];
                var indTime = GetTime(swimmers[number], i, course);
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
            return permutation.Select((n, index) => new RelaySwimmer
            {
                Position = index,
                FirstName = swimmers[n].FirstName,
                LastName = swimmers[n].LastName,
                Age = DateTime.Today.Year - swimmers[n].BirthYear,
                Time = GetTime(swimmers[n], index, course)
            }).ToList();
        }

        private double GetTime(Swimmer swimmer, int index, Course course)
        {
            switch (course)
            {
                case Course.Long:
                    switch (index)
                    {
                        case 0:
                            return swimmer.LongCourseTimes.Backstroke50M;
                        case 1:
                            return swimmer.LongCourseTimes.Breaststroke50M;
                        case 2:
                            return swimmer.LongCourseTimes.Butterfly50M;
                        case 3:
                            return swimmer.LongCourseTimes.Freestyle50M;
                        default:
                            return 999;
                    }

                case Course.Short:
                    switch (index)
                    {
                        case 0:
                            return swimmer.ShortCourseTimes.Backstroke50M;
                        case 1:
                            return swimmer.ShortCourseTimes.Breaststroke50M;
                        case 2:
                            return swimmer.ShortCourseTimes.Butterfly50M;
                        case 3:
                            return swimmer.ShortCourseTimes.Freestyle50M;
                        default:
                            return 999;
                    }

                default:
                    return 999;
            }
        }
    }
}
