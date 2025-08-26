using System.Collections.Generic;
using System.Threading.Tasks;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IRecordService
    {
        List<Record> GetRelayRecords();
    }
}
