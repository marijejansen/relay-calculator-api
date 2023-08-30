using System.Collections.Generic;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IGroupService
    {
        int GetAge(List<Swimmer> swimmers, int? forYear);

        int GetAgeGroupForOrder(int[] order, List<Swimmer> swimmers, int? forYear);

        Gender GetGenderGroup(int[] order, List<Swimmer> swimmers);
    }
}
