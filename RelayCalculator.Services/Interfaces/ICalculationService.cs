using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services.Interfaces
{
    public interface ICalculationService
    {
        List<RelayTeam> BestRelayTeams(List<Swimmer> swimmers, RelayType relayType, Course courseType);

        RelayTeam GetBestTeam(List<Swimmer> swimmers, List<int[]> possibleTeams, RelayType relayType, Course course);
    }
}
