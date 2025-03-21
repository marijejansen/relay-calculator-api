﻿using System.Collections.Generic;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Models;
using System.Threading.Tasks;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IClubRecordService
    {
        Task AddToStorage(ClubRecord clubRecord);
        Task<IEnumerable<ClubRecord>> GetAllFromStorage();
        Task<List<ClubRecord>> GetNewRecordsFromMeetResults(SwimmerMeetResult swimmerMeetResult);
        Task<List<ClubRecord>> GetNewRelayRecordsFromRelayMeetResult(RelayMeetResult relayMeetResult);
        Task UpdateRecordsInStorage(IEnumerable<ClubRecord> clubRecords);
        Task<IEnumerable<ClubRecord>> CompareHistoryWithStorageRecords();
    }
}
