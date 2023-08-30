using System.Collections.Generic;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IPermutationService
    {
        List<int[]> GetPermutations(int number);

        List<int> GetListNumbers(int number);

        List<int[]> CreatePermutations(List<int> possibleNumbers, List<int> newList = null,
            List<int[]> totalList = null);

    }
}
