using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
