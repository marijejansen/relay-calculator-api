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
        public double GetBestTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;

            //TODO: dit moet beter kunnen
            time += course == Course.Long ? swimmers[0].LongCourseTimes.Backstroke100M :
                swimmers[1].ShortCourseTimes.Backstroke100M;
            time += course == Course.Long ? swimmers[0].LongCourseTimes.Breaststroke100M :
                swimmers[1].ShortCourseTimes.Breaststroke100M;
            time += course == Course.Long ? swimmers[0].LongCourseTimes.Butterfly100M :
                swimmers[1].ShortCourseTimes.Butterfly100M;
            time += course == Course.Long ? swimmers[0].LongCourseTimes.Freestyle100M :
                swimmers[1].ShortCourseTimes.Freestyle100M;

            return time;
        }
    }
}
