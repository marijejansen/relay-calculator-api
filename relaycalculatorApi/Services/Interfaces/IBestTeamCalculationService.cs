using System.Collections.Generic;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IBestTeamCalculationService
    {
        double GetTime(int[] permutation, List<Swimmer> swimmers, Course course);

        IEnumerable<RelaySwimmer> GetRelaySwimmers(List<Swimmer> swimmers, Course course);
    }
}
