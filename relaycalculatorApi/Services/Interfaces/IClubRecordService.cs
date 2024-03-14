using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Models;
using System.Threading.Tasks;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IClubRecordService
    {
        Task AddToStorage(ClubRecord clubRecord);
        Task GetFromFile();
        Task CheckForRecords(SwimmerMeetResult swimmerMeetResult);
    }
}
