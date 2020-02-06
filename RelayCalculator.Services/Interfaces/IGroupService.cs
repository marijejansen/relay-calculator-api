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
        int GetAgeGroup(int[] order, List<Swimmer> swimmers);
        Gender GetGenderGroup(int[] order, List<Swimmer> swimmers);
        List<Swimmer> GetSwimmersByPermutation(int[] permutation, List<Swimmer> swimmers);
        List<RelaySwimmer> GetRelaySwimmersByPermutation(int[] permutation, List<Swimmer> swimmers);

    }
}
