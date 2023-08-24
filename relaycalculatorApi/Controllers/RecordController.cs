using Microsoft.AspNetCore.Mvc;
using RelayCalculator.Api.Mapper;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
