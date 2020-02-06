using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services
{
    class Freestyle400Relay : IBestTeamCalculationService
    {
        public double GetBestTime(int[] permutation, List<Swimmer> swimmers)
        {
            var time = 0.0;
            foreach (var position in permutation)
            {
                time += swimmers[position].ShortCourseTimes.Freestyle100M;
            }

            return time;
        }
    }
}
