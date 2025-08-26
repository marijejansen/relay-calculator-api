using System.Collections.Generic;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface ICalculationService
    {
        List<RelayTeam> BestRelayTeams(CalculationModel calculationModel);

        RelayTeam? GetBestTeam(List<int[]> possibleTeams, CalculationModel calculationModel);
    }
}
