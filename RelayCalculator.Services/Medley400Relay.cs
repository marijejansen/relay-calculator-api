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
    class Medley400Relay : IBestTeamCalculationService
    {
        public double GetTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;
            foreach (var i in permutation)
            {
                var indTime = GetTime(swimmers[i], i, course);
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
            return permutation.Select(n => new RelaySwimmer
            {
                FirstName = swimmers[n].FirstName,
                LastName = swimmers[n].LastName,
                Age = DateTime.Today.Year - swimmers[n].BirthYear,
                Time = GetTime(swimmers[n], n, course)
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
                            return swimmer.LongCourseTimes.Backstroke100M;
                        case 1:
                            return swimmer.LongCourseTimes.Breaststroke100M;
                        case 2:
                            return swimmer.LongCourseTimes.Butterfly100M;
                        case 3:
                            return swimmer.LongCourseTimes.Freestyle100M;
                        default:
                            return 999;
                    }

                case Course.Short:
                    switch (index)
                    {
                        case 0:
                            return swimmer.ShortCourseTimes.Backstroke100M;
                        case 1:
                            return swimmer.ShortCourseTimes.Breaststroke100M;
                        case 2:
                            return swimmer.ShortCourseTimes.Butterfly100M;
                        case 3:
                            return swimmer.ShortCourseTimes.Freestyle100M;
                        default:
                            return 999;
                    }

                default:
                    return 999;
            }
        }
    }
}
