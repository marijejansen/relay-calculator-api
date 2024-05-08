using System.Collections.Generic;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Models;
using System.Threading.Tasks;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IClubRecordService
    {
        Task AddToStorage(ClubRecord clubRecord);
        Task<IEnumerable<ClubRecord>> GetAllFromStorage();
        Task GetFromFile();
        Task<List<ClubRecord>> CheckAndGetNewRecords(SwimmerMeetResult swimmerMeetResult);
    }
}
