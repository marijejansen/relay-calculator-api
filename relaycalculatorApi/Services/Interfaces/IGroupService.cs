using System.Collections.Generic;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services.Interfaces
{
    public interface IGroupService
    {
        int GetAge(List<Swimmer> swimmers, int? forYear);

        int GetAgeGroupForOrder(int[] order, List<Swimmer> swimmers, int? forYear);

        Gender GetGenderGroup(int[] order, List<Swimmer> swimmers);
    }
}
