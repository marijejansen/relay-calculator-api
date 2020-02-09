using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
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
        private readonly ISwimTimeService _swimTimeService;
        private readonly ISearchSwimmerService _searchSwimmerService;

        public SwimmerDataController(ISwimTimeService crawlSwimTimeService, ISearchSwimmerService searchSwimmerService)
        {
            _swimTimeService = crawlSwimTimeService;
            _searchSwimmerService = searchSwimmerService;
        }

        [HttpGet]
        [Route("searchSwimmers")]
        public async Task<List<Swimmer>> GetSwimmersByNames(string firstName, string lastName)

        {
            return await _searchSwimmerService.FindSwimmersByName(firstName, lastName);
        }

        [HttpGet]
        [Route("getTimesShortCourse")]
        public async Task<CourseTimes> GetTimesBySwimmerIdShortCourse(int id, int fromYear)
        {
            return await _swimTimeService.SelectTimesByCourse(id, fromYear, Course.Short);
        }

        [HttpGet]
        [Route("getTimesLongCourse")]
        public async Task<CourseTimes> GetTimesBySwimmerIdLongCourse(int id, int fromYear)
        {
            return await _swimTimeService.SelectTimesByCourse(id, fromYear, Course.Long);
        }
    }
}
