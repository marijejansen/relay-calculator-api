using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services
{
    class Medley400Relay : IBestTeamCalculationService
    {
        public double GetBestTime(int[] permutation, List<Swimmer> swimmers)
        {
            var time = 0.0;

            //TODO: dit moet beter kunnen
            time += swimmers[permutation[0]].ShortCourseTimes.Backstroke100M;
            time += swimmers[permutation[1]].ShortCourseTimes.Breaststroke100M;
            time += swimmers[permutation[2]].ShortCourseTimes.Butterfly100M;
            time += swimmers[permutation[3]].ShortCourseTimes.Freestyle100M;

            return time;
        }
    }
}
