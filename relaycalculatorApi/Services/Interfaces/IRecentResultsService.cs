using RelayCalculator.Api.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IRecentResultsService
    {
        Task<IEnumerable<ClubRecord>> GetNewRecordsFromSwimrankings(DateTime? fromDate);
    }
}
