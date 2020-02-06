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
    class Medley200Relay : IBestTeamCalculationService
    {
        public double GetBestTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;

            //TODO: dit moet beter kunnen
            time += course == Course.Long ? swimmers[0].LongCourseTimes.Backstroke50M :
                swimmers[1].ShortCourseTimes.Backstroke50M;
            time += course == Course.Long ? swimmers[0].LongCourseTimes.Breaststroke50M :
                swimmers[1].ShortCourseTimes.Breaststroke50M;
            time += course == Course.Long ? swimmers[0].LongCourseTimes.Butterfly50M :
                swimmers[1].ShortCourseTimes.Butterfly50M;
            time += course == Course.Long ? swimmers[0].LongCourseTimes.Freestyle50M :
                swimmers[1].ShortCourseTimes.Freestyle50M;

            return time;
        }
    }
}
