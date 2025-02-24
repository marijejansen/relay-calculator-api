using Azure.Data.Tables;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Entities;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RelayCalculator.Api.Utils;
using Microsoft.Extensions.Configuration;


namespace RelayCalculator.Api.Services
{
    public class ClubRecordService : IClubRecordService
    {
        private readonly TableClient _tableClient;
        private readonly IClubRecordFileService _clubRecordFileService;

        public ClubRecordService(IClubRecordFileService clubRecordFileService, IConfiguration configuration) {
            string connectionString = configuration.GetConnectionString("StorageConnectionString"); 
            var tableName = "clubRecords";
            _tableClient = new TableClient(connectionString, tableName);
            // Create the table if it doesn't already exist to verify we've successfully authenticated.
            _tableClient.CreateIfNotExists();
            _clubRecordFileService = clubRecordFileService;
        }

        // gets all records from storage
        public async Task<IEnumerable<ClubRecord>> GetAllFromStorage()
        {
            var newRecords = new List<ClubRecord>();
            var recordsAsync = _tableClient.QueryAsync<RecordEntity>();
            await foreach (var record in recordsAsync)
            {
                var splitPartionKey = record.PartitionKey.Split("_");
                var isRelay = splitPartionKey[0] == "R";
                var startIndex = isRelay ? 1 : 0;
                var splitRowKey = record.RowKey.Split("_");
                newRecords.Add(new ClubRecord()
                {
                    AgeGroup = int.Parse(splitPartionKey[startIndex+1]),
                    Course = (Course)Enum.Parse(typeof(Course), splitPartionKey[startIndex+2]),
                    Date = record.RecordDate.Date,
                    Gender = (Gender)Enum.Parse(typeof(Gender), splitPartionKey[startIndex]),
                    Stroke = (Stroke)Enum.Parse(typeof(Stroke), splitRowKey[0]),
                    Distance = (Distance)Enum.Parse(typeof(Distance), splitRowKey[1]),
                    IsRelay = isRelay,
                    Time = record.Time,
                    Name = record.Name
                });
            }
            return newRecords;
        }

        public async Task UpdateRecordsInStorage(IEnumerable<ClubRecord> clubRecords)
        {
            // filter only one record per gender + age + course + stroke + distance
            var filteredRecords = clubRecords.GroupBy(r => new { r.Gender, r.AgeGroup, r.Course, r.Stroke, r.Distance })
                .Select(rec => rec.OrderBy(record => record.Time).First());

            var tasks = filteredRecords.Select(async record =>
                {
                    await AddToStorage(record);
                }
            );

            await Task.WhenAll(tasks);
        }

        // adds a single record to storage
        public async Task AddToStorage(ClubRecord clubRecord)
        {
            var entity = new RecordEntity(clubRecord);
            await _tableClient.UpsertEntityAsync(entity);
        }

        // gets new records from meet results
        public async Task<List<ClubRecord>> GetNewRecordsFromMeetResults(SwimmerMeetResult swimmerMeetResult)
        {
            var ageGroup = Math.Floor((double) ((swimmerMeetResult.Date.Year - swimmerMeetResult.BirthYear) / 5)) * 5;
            var partitionKey = $"{swimmerMeetResult.Gender}_{ageGroup}_{swimmerMeetResult.Course}";
            var filter = $"PartitionKey eq '{partitionKey}'";
            var recordsAsync =  _tableClient.QueryAsync<RecordEntity>(filter); 
            var newRecords = new List<ClubRecord>();


            await foreach (var record in recordsAsync)
            {
                var newResult = swimmerMeetResult.EventResults.FirstOrDefault(e =>
                    $"{e.SwimEvent.Stroke}_{(int)e.SwimEvent.Distance}" == record.RowKey);
                if (newResult != null)
                {
                    if (((record.Time > 0 && newResult.Time < record.Time && record.Time - newResult.Time > 0.001) || record.Time == 0) && newResult.Time > 0)
                    {
                        Console.WriteLine(
                            $"{swimmerMeetResult.FirstName} {swimmerMeetResult.LastName} " +
                            $"{SwimmerUtils.GenderToDutchString(swimmerMeetResult.Gender)}{ageGroup}+ " +
                            $"{(int)newResult.SwimEvent.Distance}m {SwimmerUtils.StrokeToDutchString(newResult.SwimEvent.Stroke)} " +
                            $"{SwimmerUtils.CourseToDutchShorthand(swimmerMeetResult.Course).ToLower()}: " +
                            $"van {SwimmerUtils.ConvertDoubleToTimeString(record.Time)} naar {SwimmerUtils.ConvertDoubleToTimeString(newResult.Time)}");

                        newRecords.Add(new ClubRecord()
                        {
                            AgeGroup = (int)ageGroup,
                            Course = swimmerMeetResult.Course,
                            Date = swimmerMeetResult.Date,
                            Distance = newResult.SwimEvent.Distance,
                            Gender = swimmerMeetResult.Gender,
                            Name = swimmerMeetResult.FirstName + " " + swimmerMeetResult.LastName,
                            Stroke = newResult.SwimEvent.Stroke,
                            Time = newResult.Time
                        });
                    }

                }
            }

            return newRecords;
        }

        public async Task<List<ClubRecord>> GetNewRelayRecordsFromRelayResults(RelayMeetResult relayMeetResult)
        {
            return new List<ClubRecord>() { };
        }

        public async Task<IEnumerable<ClubRecord>> CompareHistoryWithStorageRecords()
        {
            var allHistoryRecords = await _clubRecordFileService.GetClubRecordsHistoryFromFile();
            var bestRecords = await GetAllFromStorage();

            var recordsToBeUpdated = new List<ClubRecord>();

            foreach (var record in bestRecords)
            {
                var historyRecordsMatch = allHistoryRecords.Where(hisRec =>
                    hisRec.AgeGroup == record.AgeGroup &&
                    hisRec.Gender == record.Gender &&
                    hisRec.Course == record.Course &&
                    hisRec.Distance == record.Distance &&
                    hisRec.Stroke == record.Stroke).ToList();
                if (historyRecordsMatch.Any())
                {
                    //var lastFromHistoryFilter = historyRecordsMatch.Max(hisRec => hisRec.Date);
                    var fastestFromHistory = historyRecordsMatch.OrderBy(hist => hist.Time).First();
                    if (Math.Abs(fastestFromHistory.Time - record.Time) > 0.001)
                    {
                        Console.WriteLine($"difference found for: {record.Gender}{record.AgeGroup} {record.Course}: {(int)record.Distance}{SwimmerUtils.StrokeToDutchString(record.Stroke)} => " +
                                          $"{record.Name} ({SwimmerUtils.ConvertDoubleToTimeString(record.Time)}) {record.Date} + {fastestFromHistory.Name} ({SwimmerUtils.ConvertDoubleToTimeString(fastestFromHistory.Time)}) {fastestFromHistory.Date}");
                        recordsToBeUpdated.Add(fastestFromHistory);
                    }

                }
                else if(record.Time > 0)
                {
                    recordsToBeUpdated.Add(record); 
                    Console.WriteLine($"No record found in history for: {record.Gender}{record.AgeGroup} {record.Course}: {(int)record.Distance}{SwimmerUtils.StrokeToDutchString(record.Stroke)}");
                }
            }
            return recordsToBeUpdated;
        }

    }
}
