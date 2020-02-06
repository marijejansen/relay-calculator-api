using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services.Interfaces
{
    public interface ICalculationService
    {
        string GetString();

        List<RelayTeam> BestRelayTeams(List<Swimmer> swimmers, RelayType relayType);

        RelayTeam GetBestTeam(List<Swimmer> swimmers, List<int[]> possibleTeams, RelayType relayType);
    }
}
