using RelayCalculator.Api.Models;
using System;
using System.Threading.Tasks;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface ISwimmerStatService
    {
        Task<SwimmerStatModel> GetSwimmerStats(int swimmerId, DateTime? startDate);
    }
}
