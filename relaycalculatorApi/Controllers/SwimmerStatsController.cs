using Microsoft.AspNetCore.Mvc;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Interfaces;
using System.Threading.Tasks;

namespace RelayCalculator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwimmerStatsController : ControllerBase
    {
        private readonly ISwimmerStatService swimmerStatService;
        public SwimmerStatsController(ISwimmerStatService swimmerStatService) {
            this.swimmerStatService = swimmerStatService;
        }

        [HttpGet]
        [Route("{swimmerId}")]
        public Task<SwimmerStatModel> GetSwimmerStats([FromRoute] int swimmerId) { 

            return swimmerStatService.GetSwimmerStats(swimmerId, new System.DateTime(2023, 1, 1)); 

        }
    }
}
