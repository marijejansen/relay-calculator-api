using System.Collections.Generic;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services.Interfaces
{
    public interface ICalculationService
    {
        List<RelayTeam> BestRelayTeams(CalculationModel calculationModel);

        RelayTeam GetBestTeam(List<int[]> possibleTeams, CalculationModel calculationModel);
    }
}
