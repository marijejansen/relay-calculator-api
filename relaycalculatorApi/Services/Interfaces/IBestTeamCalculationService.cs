using System.Collections.Generic;
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
