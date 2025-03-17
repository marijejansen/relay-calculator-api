using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;
using System;
using RelayCalculator.Api.Utils;
using System.Linq;

namespace RelayCalculator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService recordsService;
        //private readonly IClubRecordService clubRecordService;
        //private readonly IClubRecordFileService clubRecordFileService;
        //private readonly IRecentResultsService recentResultsService;


        public RecordController(IRecordService recordsService
            //, IClubRecordService clubRecordService, 
            //IRecentResultsService recentResultsService, 
            //IClubRecordFileService clubRecordFileService
            )
        {
            this.recordsService = recordsService;
            //this.clubRecordService = clubRecordService;
            //this.recentResultsService = recentResultsService;
            //this.clubRecordFileService = clubRecordFileService; 
        }

        [HttpGet]
        [Route("relays")]
        public async Task<List<Record>> GetRelayRecords()
        {
            var records = await recordsService.GetRelayRecords();
            return records;
        }

        //[HttpGet]
        //[Route("clubrecords")]
        //public async Task<IEnumerable<ClubRecord>> GetAllClubRecords()
        //{
        //    return await clubRecordService.GetAllFromStorage();
        //}


        //[HttpPost]
        //[Route("clubrecords")]
        //public async Task PostRecord(ClubRecord clubRecord)
        //{
        //    await clubRecordService.AddToStorage(clubRecord);
        //}

        //[HttpPost]
        //[Route("getfromfile")]
        //public async Task GetFromFile()
        //{
        //    // make sure the history file is uptodate with the storage records
        //    //var recordsToUpdate = await clubRecordService.CompareHistoryWithStorageRecords();
        //    //await clubRecordFileService.CreateHistoryFileFromRecords(recordsToUpdate.ToList());

        //    // * * * *  use the records in the storage to create a records file
        //    //var recordsFromStorage = await clubRecordService.GetAllFromStorage();
        //    //await clubRecordFileService.CreateRecordFileFromRecords(recordsFromStorage.ToList());

        //    //await clubRecordFileService.GetRecordsFromFile();
        //    //var recordsToBeUpdated = await clubRecordService.CompareHistoryWithStorageRecords();
        //    //await clubRecordService.UpdateRecordsInStorage(recordsToBeUpdated);
        //    //var records = await clubRecordFileService.GetClubRecordsHistoryFromFile();

        //    // current relayrecords from historyfile will be stored in storage
        //    var relayRecordsHistory = clubRecordFileService.GetRelayHistoryFromFile();
        //    await clubRecordService.UpdateRecordsInStorage(relayRecordsHistory);

        //}

        //[HttpPost]
        //[Route("updateClubRecords")]
        //public async Task<IEnumerable<ClubRecord>> UpdateClubRecords(DateTime fromDate, bool? fromList)
        //{
        //    // nieuwe records vanaf september staan er nog niet in
        //    var records = (await recentResultsService.GetNewRecordsFromSwimrankings(fromDate, fromList ?? false)).ToList();

        //    if (records.Any())
        //    {
        //        Console.WriteLine("update these records? Y/N");
        //        var updateRecords = Console.ReadLine();
        //        if(updateRecords?.ToLower() == "y")
        //        {
        //            await clubRecordFileService.CreateHistoryFileFromRecords(records.ToList());
        //            await clubRecordService.UpdateRecordsInStorage(records);
                    
        //            var allRecords = await clubRecordService.GetAllFromStorage();
        //            await clubRecordFileService.CreateRecordFileFromRecords(allRecords.ToList());
        //        }
        //    }

        //    return records;
        //}
    }
}
