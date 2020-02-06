using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services.Interfaces
{
    public interface IBestTeamCalculationService
    {
        double GetBestTime(int[] permutation, List<Swimmer> swimmers);
    }
}
