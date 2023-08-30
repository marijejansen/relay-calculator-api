using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService recordsService;


        public RecordController(IRecordService recordsService)
        {
            this.recordsService = recordsService;
        }
        [HttpGet]
        [Route("")]
        public async Task<List<Swimmer>> GetRecords()
        {
            await recordsService.GetRecords();
            return new List<Swimmer>();
        }
    }
}
