using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelayCalculator.Services.Interfaces
{
    public interface IPermutationService
    {
        List<int[]> GetPermutations(int number);

        List<int> GetListNumbers(int number);

        List<int[]> CreatePermutations(List<int> possibleNumbers, List<int> newList = null,
            List<int[]> totalList = null);

    }
}
