using Azure;
using Azure.Data.Tables;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Entities;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;
using Spire.Xls;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RelayCalculator.Api.Utils;


namespace RelayCalculator.Api.Services
{
    public class ClubRecordService : IClubRecordService
    {
        readonly private TableClient _tableClient;
        public ClubRecordService() {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=relaycalculatorstorage;AccountKey=RPHQsrFZbfUbRtwMqnk1OOZDG5mQBXVPaakvVm5U7O1uKZNG/PPaYFxbTG6wtKindAUbOI+ZVUIH+ASt507mXg==;EndpointSuffix=core.windows.net";
            var tableName = "clubRecords";
            _tableClient = new TableClient(connectionString, tableName);
            // Create the table if it doesn't already exist to verify we've successfully authenticated.
            _tableClient.CreateIfNotExists();
        }

        public async Task AddToStorage(ClubRecord clubRecord)
        {
            var entity = new RecordEntity(clubRecord);
            await _tableClient.AddEntityAsync(entity);
        }

        public async Task CheckForRecords(SwimmerMeetResult swimmerMeetResult)
        {
            var ageGroup = Math.Floor((double) ((swimmerMeetResult.Date.Year - swimmerMeetResult.BirthYear) / 5)) * 5;
            var partitionKey = $"{swimmerMeetResult.Gender}_{ageGroup}_{swimmerMeetResult.Course}";
            var filter = $"PartitionKey eq '{partitionKey}'";
            var recordsAsync =  _tableClient.QueryAsync<RecordEntity>(filter);

            await foreach (var record in recordsAsync)
            {
                var matchingEvent = swimmerMeetResult.EventResults.FirstOrDefault(e =>
                    $"{e.SwimEvent.Stroke}_{(int)e.SwimEvent.Distance}" == record.RowKey);
                if (matchingEvent != null)
                {
                    if (((record.Time > 0 && matchingEvent.Time < record.Time) || record.Time == 0) && matchingEvent.Time > 0)
                    {
                        Console.WriteLine(
                            $"Nieuw record voor {swimmerMeetResult.FirstName} {swimmerMeetResult.LastName} " +
                            $"{SwimmerUtils.GenderToDutchString(swimmerMeetResult.Gender)}{ageGroup}+ " +
                            $"op de {(int)matchingEvent.SwimEvent.Distance}m {SwimmerUtils.StrokeToDutchString(matchingEvent.SwimEvent.Stroke)} " +
                            $"{SwimmerUtils.CourseToDutchShorthand(swimmerMeetResult.Course)}: " +
                            $"van {SwimmerUtils.ConvertDoubleToTimeString(record.Time)} naar {SwimmerUtils.ConvertDoubleToTimeString(matchingEvent.Time)}");
                    }

                    //Console.WriteLine(
                    //    $"{swimmerMeetResult.FirstName} {swimmerMeetResult.LastName} " +
                    //    $"{SwimmerUtils.GenderToDutchString(swimmerMeetResult.Gender)}{ageGroup}+ " +
                    //    $"op de {(int)matchingEvent.SwimEvent.Distance}m {SwimmerUtils.StrokeToDutchString(matchingEvent.SwimEvent.Stroke)} " +
                    //    $"{SwimmerUtils.CourseToDutchShorthand(swimmerMeetResult.Course)}: " +
                    //    $"tijd: {SwimmerUtils.ConvertDoubleToTimeString(matchingEvent.Time)} (record = {SwimmerUtils.ConvertDoubleToTimeString(record.Time)})");

                }
                var x = 0;
            }
        }

        public async Task GetFromFile()
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile("Clubrecords actueel.xlsx");

            int[] ages = {20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75};
            Gender[] genders = { Gender.Female, Gender.Male };

            foreach(Gender gender in genders) {
                var start = gender == Gender.Female ? 0 : 12;
                var end = gender == Gender.Female ? 12 : 22;
                for (int a = start; a < end; a++)
                {
                    var sheet = workbook.Worksheets[a];

                    for (int x = 0; x < 2; x++)
                    {
                        var course = x == 0 ? Course.Long : Course.Short;
                        var startCol = x == 0 ? 0 : 5;

                        for (int i = 2; i < 24; i++)
                        {
                            var swimEvent = sheet.Rows[i].Columns[startCol].Value;
                            var time = sheet.Rows[i].Columns[startCol + 1].Value;
                            var name = sheet.Rows[i].Columns[startCol + 2].Value;
                            var date = sheet.Rows[i].Columns[startCol + 3].Value;
                            if (swimEvent == "")
                            {
                                continue;
                            }

                            try
                            {
                                var entity = new RecordEntity(new ClubRecord
                                {
                                    AgeGroup = ages[a % 12],
                                    Course = course,
                                    Gender = gender,
                                    Name = name,
                                    Distance = GetDistance(swimEvent),
                                    Stroke = GetStroke(swimEvent),
                                    Time = GetTime(time),
                                    Date = GetDate(date)
                                });
                                await _tableClient.UpsertEntityAsync(entity);

                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"age: {ages[1 - 12]} {swimEvent} {course}");
                            }
                        }
                    }
                }
            }

            //var x = sheet.ExportDataTable();

            var y = 0;
            //var entity = new RecordEntity(clubRecord);
            //await _tableClient.AddEntityAsync(entity);
        }

        private Distance GetDistance(string eventString)
        {
            var distance = eventString.Split(' ')[0];
            switch(distance)
            {
                case "50":
                    return Distance.Fifty;
                case "100":
                    return Distance.Hundred;
                case "200":
                    return Distance.TwoHundred;
                case "400":
                    return Distance.FourHundred;
                case "800":
                    return Distance.EightHundred;
                case "1500":
                    return Distance.FifteenHundred;
                default:
                    return Distance.TwentyFive;

            }
        }
        private Stroke GetStroke(string eventString)
        {
            var stroke = eventString.Split(' ')[1];
            switch (stroke)
            {
                case "vrij":
                    return Stroke.Freestyle;
                case "rug":
                    return Stroke.Backstroke;
                case "school":
                    return Stroke.Breaststroke;
                case "vlinder":
                    return Stroke.Butterfly;
                case "wisselslag":
                case "wissel":
                    return Stroke.Medley;
                default:
                    return Stroke.Unknown;

            }
        }

        private DateTime GetDate(string dateString)
        {
            if (dateString == "") return DateTime.Now;
            var split = dateString.Split("/");
            return new DateTime(int.Parse(split[2].Split(" ")[0]), int.Parse(split[1]), int.Parse(split[0]));
        }

        private double GetTime(string timeString)
        {
            if (timeString == "") return 0;
            var split = timeString.Split(new string[] { ".", "," }, StringSplitOptions.RemoveEmptyEntries);
            var hunSec = int.Parse(split[split.Length-1]);
            var sec = int.Parse(split[split.Length - 2]);

            double time = sec + ((double)hunSec / 100);

            if(split.Length == 3)
            {
                time += (int.Parse(split[0]) * 60);
            }
            return time;
        }
    }
}
