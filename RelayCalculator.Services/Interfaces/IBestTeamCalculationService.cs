using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services.Interfaces
{
    public interface IBestTeamCalculationService
    {
        double GetTime(int[] permutation, List<Swimmer> swimmers, Course course);

        IEnumerable<RelaySwimmer> GetRelaySwimmers(List<Swimmer> swimmers, Course course);
    }
}
