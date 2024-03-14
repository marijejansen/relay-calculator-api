using System;
using System.Threading.Tasks;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IRecentResultsService
    {
        Task GetRecentResults(DateTime? fromDate);
    }
}
