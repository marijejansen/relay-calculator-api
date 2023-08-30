using System.Collections.Generic;
using System.Threading.Tasks;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface ISearchSwimmerService
    {
        Task<List<Swimmer>> FindSwimmersByName(string firstName, string lastName);
    }
}
