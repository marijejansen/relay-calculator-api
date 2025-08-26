using System;
using System.Collections.Generic;
using System.Linq;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services
{
    public class Medley400Relay : IBestTeamCalculationService
    {
        public double GetTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;
            for (var i = 0; i < permutation.Length; i++)
            {
                var number = permutation[i];
                var indTime = this.GetTime(swimmers[number], i, course);
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
            return swimmers.Select((swimmer, index) => new RelaySwimmer
            {
                ID = swimmer.ID,
                Position = index,
                FirstName = swimmer.FirstName,
                LastName = swimmer.LastName,
                Age = DateTime.Today.Year - swimmer.BirthYear,
                Time = this.GetTime(swimmer, index, course)
            }).ToList();
        }

        private double? GetTime(Swimmer swimmer, int index, Course course)
        {
            switch (course)
            {
                case Course.Long:
                    switch (index)
                    {
                        case 0:
                            return swimmer.LongCourseTimes?.Backstroke100M;
                        case 1:
                            return swimmer.LongCourseTimes?.Breaststroke100M;
                        case 2:
                            return swimmer.LongCourseTimes?.Butterfly100M;
                        case 3:
                            return swimmer.LongCourseTimes?.Freestyle100M;
                        default:
                            return 999;
                    }

                case Course.Short:
                    switch (index)
                    {
                        case 0:
                            return swimmer.ShortCourseTimes?.Backstroke100M;
                        case 1:
                            return swimmer.ShortCourseTimes?.Breaststroke100M;
                        case 2:
                            return swimmer.ShortCourseTimes?.Butterfly100M;
                        case 3:
                            return swimmer.ShortCourseTimes?.Freestyle100M;
                        default:
                            return 999;
                    }

                default:
                    return 999;
            }
        }
    }
}
