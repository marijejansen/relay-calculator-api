using System.Collections.Generic;
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
        private readonly ISwimTimeService _swimTimeService;
        private readonly ISearchSwimmerService _searchSwimmerService;

        public SwimmerDataController(ISwimTimeService crawlSwimTimeService, ISearchSwimmerService searchSwimmerService)
        {
            _swimTimeService = crawlSwimTimeService;
            _searchSwimmerService = searchSwimmerService;
        }

        /// <summary>
        /// Search for swimmers by first and last name.
        /// </summary>
        /// <returns>
        /// Returns a list of swimmers that meet the search.
        /// </returns>
        [HttpGet]
        [Route("searchSwimmers")]
        public async Task<List<Swimmer>> GetSwimmersByNames(string firstName, string lastName)

        {
            return await _searchSwimmerService.FindSwimmersByName(firstName, lastName);
        }

        /// <summary>
        /// Gets the fastest times on short course for the swimmer from a given year
        /// </summary>
        /// <returns>
        /// Returns times for all strokes in seconds.
        /// </returns>
        [HttpGet]
        [Route("getTimesShortCourse")]
        public async Task<CourseTimes> GetTimesBySwimmerIdShortCourse(int id, int fromYear,
            int? numberOfYearsBackIfNoResult, bool? getAllTimes)
        {
            return await _swimTimeService.SelectTimesByCourse(id, fromYear, Course.Short, numberOfYearsBackIfNoResult, getAllTimes);
        }


        /// <summary>
        /// Gets the fastest times on long course for the swimmer from a given year
        /// </summary>
        /// <returns>
        /// Returns times for all strokes in seconds.
        /// </returns>
        [HttpGet]
        [Route("getTimesLongCourse")]
        public async Task<CourseTimes> GetTimesBySwimmerIdLongCourse(int id, int fromYear, int? numberOfYearsBackIfNoResult, bool? getAllTimes)
        {
            return await _swimTimeService.SelectTimesByCourse(id, fromYear, Course.Long, numberOfYearsBackIfNoResult, getAllTimes);
        }
    }
}
