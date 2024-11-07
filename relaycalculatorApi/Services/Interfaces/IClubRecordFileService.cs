using System.Collections.Generic;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Models;
using System.Threading.Tasks;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IClubRecordFileService
    {
        Task<List<ClubRecord>> GetRecordsFromFile();
        Task<List<ClubRecord>> GetClubRecordsHistoryFromFile();
        Task CreateHistoryFileFromRecords(List<ClubRecord> newRecords);
        Task CreateRecordFileFromHistory();
    }
}
