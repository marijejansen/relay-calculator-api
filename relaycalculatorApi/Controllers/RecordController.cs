using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;
using System;
using RelayCalculator.Api.Utils;

namespace RelayCalculator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService recordsService;
        private readonly IClubRecordService clubRecordService;
        private readonly IRecentResultsService recentResultsService;


        public RecordController(IRecordService recordsService, IClubRecordService clubRecordService, IRecentResultsService recentResultsService)
        {
            this.recordsService = recordsService;
            this.clubRecordService = clubRecordService;
            this.recentResultsService = recentResultsService;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<Swimmer>> GetRecords()
        {
            await recordsService.GetRecords();
            return new List<Swimmer>();
        }

        [HttpPost]
        [Route("clubrecords")]
        public async Task PostRecord(ClubRecord clubRecord)
        {
            await clubRecordService.AddToStorage(clubRecord);
        }

        [HttpPost]
        [Route("getfromfile")]
        public async Task GetFromFile()
        {
            await clubRecordService.GetFromFile();
            //await recentResultsService.GetRecentResults();
        }

        [HttpPost]
        [Route("updateClubRecords")]
        public async Task UpdateClubRecords(DateTime fromDate)
        {
            //SwimmerUtils.GetNameArrayFromString("BON-ROSENBRAND van, Lidia");
            //SwimmerUtils.GetNameArrayFromString("JANSEN, Marije");
            //SwimmerUtils.GetNameArrayFromString("LAAN van der, Christina");
            //SwimmerUtils.ConvertDoubleToTimeString(123.34);
            //SwimmerUtils.ConvertDoubleToTimeString(27.34);
            //SwimmerUtils.ConvertDoubleToTimeString(345.34);
            //SwimmerUtils.ConvertDoubleToTimeString(345.3);
            //SwimmerUtils.ConvertDoubleToTimeString(45.01);
            //SwimmerUtils.ConvertDoubleToTimeString(60.01);

            await recentResultsService.GetRecentResults(fromDate);
        }
    }
}
