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
        public double GetBestTime(int[] permutation, List<Swimmer> swimmers, Course course)
        {
            var time = 0.0;
            foreach (var position in permutation)
            {
                time += course == Course.Long ? swimmers[position].LongCourseTimes.Freestyle100M :
                    swimmers[position].ShortCourseTimes.Freestyle100M;
            }

            return time;
        }
    }
}
