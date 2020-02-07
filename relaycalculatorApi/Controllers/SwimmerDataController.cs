using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwimmerDataController : ControllerBase
    {
        private readonly ICrawlSwimTimeService _crawlSwimTimeService;

        public SwimmerDataController(ICrawlSwimTimeService crawlSwimTimeService)
        {
            _crawlSwimTimeService = crawlSwimTimeService;
        }

        [HttpGet]
        [Route("getTimesShortCourse")]
        public async Task<CourseTimes> GetTimesBySwimmerIdShortCourse(int id, int fromYear)
        {
            return await _crawlSwimTimeService.SelectTimesByCourse(id, fromYear, Course.Short);
        }

        [HttpGet]
        [Route("getTimesLongCourse")]
        public async Task<CourseTimes> GetTimesBySwimmerIdLongCourse(int id, int fromYear)
        {
            return await _crawlSwimTimeService.SelectTimesByCourse(id, fromYear, Course.Long);
        }
    }
}
