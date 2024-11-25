using System;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Models;
using RelayCalculator.Api.Services.Interfaces;
using Spire.Xls;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Utils;


namespace RelayCalculator.Api.Services
{
    public class ClubRecordFileService : IClubRecordFileService
    {
        //private readonly IClubRecordService _clubRecordService;
        //public ClubRecordFileService(IClubRecordService clubRecordService)
        //{
        //    _clubRecordService = clubRecordService;
        //}
        // gets all the records from the history file
        public Task<List<ClubRecord>> GetClubRecordsHistoryFromFile()
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile("Clubrecords history.xlsx");

            int[] ages = { 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75 };
            Course[] courses = { Course.Short, Course.Long };
            Gender[] genders = { Gender.Female, Gender.Male };

            var records = new List<ClubRecord>();

            foreach (int age in ages)
            {
                var ageGroup = $"{age}-{age + 4}";
                foreach (Course course in courses)
                {
                    foreach (Gender gender in genders)
                    {
                        var worksheetName = $"{ageGroup} {(course == Course.Short ? "kb" : "lb")} {(gender == Gender.Female ? "dames" : "heren")}";
                        var sheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name.Trim() == worksheetName);
                        if (sheet == null) continue;
                        Distance distance = Distance.Unknown;
                        Stroke stroke = Stroke.Unknown;
                        for (int x = 0; x < sheet.Rows.Length; x++)
                        {
                            var columnsForRow = sheet.Rows[x].Columns;
                            if (columnsForRow.Length == 1) continue;
                            var eventName = columnsForRow[0].Value;
                            if (eventName != "")
                            {
                                var eventStroke = SwimmerUtils.GetStrokeFromStringDutch(eventName);
                                if (eventStroke == Stroke.Unknown) continue;
                                distance = SwimmerUtils.GetDistanceFromStringDutch(eventName);
                                stroke = eventStroke;
                            }
                            var eventTime = columnsForRow?[1]?.Value;
                            if (string.IsNullOrEmpty(eventTime)) continue;
                            //var time = GetTime(eventTime);
                            var name = columnsForRow[2].Value;
                            var date = SwimmerUtils.GetDateFromDateStringShort(columnsForRow[3].Value);

                            var clubRecord = new ClubRecord
                            {
                                AgeGroup = age,
                                Course = course,
                                Gender = gender,
                                Name = name,
                                Distance = distance,
                                Stroke = stroke,
                                Time = SwimmerUtils.GetTimeFromTimeString(eventTime),
                                Date = date,
                            };
                            records.Add(clubRecord);

                        }
                    }
                }
            }

            var asdasdx = records.Where(rec => rec.Distance == Distance.FourHundred && rec.Stroke == Stroke.Medley);
            return Task.FromResult(records);
        }


        // get records from the record file
        public Task<List<ClubRecord>> GetRecordsFromFile()
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile("Clubrecords actueel.xlsx");

            int[] ages = { 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75 };
            Gender[] genders = { Gender.Female, Gender.Male };
            var clubRecords = new List<ClubRecord>();

            foreach (Gender gender in genders)
            {
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


                            clubRecords.Add(new ClubRecord
                            {
                                AgeGroup = ages[a % 12],
                                Course = course,
                                Gender = gender,
                                Name = name,
                                Distance = SwimmerUtils.GetDistanceFromStringDutch(swimEvent),
                                Stroke = SwimmerUtils.GetStrokeFromStringDutch(swimEvent),
                                Time = SwimmerUtils.GetTimeFromTimeString(time),
                                Date = SwimmerUtils.GetDateFromDateStringShort(date)
                            });

                        }
                    }
                }
            }

            return Task.FromResult(clubRecords);
        }

        public Task CreateRecordFileFromRecords(List<ClubRecord> records)
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Clear();

            List<int> ages = Constants.Individual.AgeGroups;

            Course[] courses = { Course.Short, Course.Long };
            Gender[] genders = { Gender.Female, Gender.Male };

            foreach (int age in ages)
            {
                var filteredRecordsAge = records.Where(rec => rec.AgeGroup == age).ToList();

                var ageGroup = $"{age}-{age + 4}";
                foreach (Course course in courses)
                {
                    var filteredRecordsCourse = filteredRecordsAge.Where(rec => rec.Course == course).ToList();
                    foreach (Gender gender in genders)
                    {
                        var filteredRecordsGender = filteredRecordsCourse.Where(rec => rec.Gender == gender).ToList();

                        var worksheetName =
                            $"{age}-{age + 4} {(course == Course.Short ? "kb" : "lb")} {(gender == Gender.Female ? "dames" : "heren")}";
                        Worksheet worksheet = workbook.CreateEmptySheet(worksheetName);
                        worksheet.Range["A1"].Text = worksheetName;
                        worksheet.Range["A1"].Style.Font.IsBold = true;


                        var rowNumber = 3;

                        var swimEvents = course == Course.Short
                            ? Constants.Individual.AllSwimEventsShortCourse
                            : Constants.Individual.AllSwimEventsLongCourse;
                        foreach (SwimEvent swimEvent in swimEvents)
                        {
                            worksheet.Range[$"A{rowNumber}"].Text =
                                $"{(int) swimEvent.Distance} {SwimmerUtils.StrokeToDutchString(swimEvent.Stroke)}";

                            var recordsForEvent = filteredRecordsGender?.Where(rec =>
                                rec.Distance == swimEvent.Distance && rec.Stroke == swimEvent.Stroke).ToList();
                            if (!recordsForEvent.Any())
                            {
                                rowNumber += 2;
                                continue;
                            }

                            
                            var record = recordsForEvent?.OrderBy(rec => rec.Date)?.Last();

                            worksheet.Range[$"B{rowNumber}"].Text = SwimmerUtils.ConvertDoubleToTimeString(record.Time);
                            worksheet.Range[$"C{rowNumber}"].Text = record.Name;
                            worksheet.Range[$"D{rowNumber}"].DateTimeValue = record.Date;

                            worksheet.Columns[0].ColumnWidth = 15;
                            worksheet.Columns[1].ColumnWidth = 15;
                            worksheet.Columns[2].ColumnWidth = 15;
                            worksheet.Columns[3].ColumnWidth = 15;
                            rowNumber += 2;

                        }

                    }
                }

            }

            var now = DateTime.Now;
            workbook.SaveToFile($"{now.Year}-{now.Month}-{now.Day}=Clubrecords actueel.xlsx", ExcelVersion.Version2013);
            workbook.Worksheets.RemoveAt(workbook.Worksheets.Count - 1);

            return Task.CompletedTask;

        }

        public async Task CreateHistoryFileFromRecords(List<ClubRecord> newRecords)
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Clear();

            var oldHistoryRecords = await GetClubRecordsHistoryFromFile();
            var allRecords = oldHistoryRecords.Concat(newRecords);

            List<int> ages = Constants.Individual.AgeGroups;

            Course[] courses = { Course.Short, Course.Long };
            Gender[] genders = { Gender.Female, Gender.Male };

            foreach (int age in ages)
            {
                var filteredRecordsAge = allRecords.Where(rec => rec.AgeGroup == age).ToList();

                var ageGroup = $"{age}-{age + 4}";
                foreach (Course course in courses)
                {
                    var filteredRecordsCourse = filteredRecordsAge.Where(rec => rec.Course == course).ToList();
                    foreach (Gender gender in genders)
                    {
                        var filteredRecordsGender = filteredRecordsCourse.Where(rec => rec.Gender == gender).ToList();

                        var worksheetName = $"{ageGroup} {(course == Course.Short ? "kb" : "lb")} {(gender == Gender.Female ? "dames" : "heren")}";
                        Worksheet worksheet = workbook.CreateEmptySheet(worksheetName);
                        worksheet.Range["A1"].Text = worksheetName;
                        worksheet.Range["A1"].Style.Font.IsBold = true;


                        var rowNumber = 3;

                        var swimEvents = course == Course.Short ? Constants.Individual.AllSwimEventsShortCourse : Constants.Individual.AllSwimEventsLongCourse;
                        foreach (SwimEvent swimEvent in swimEvents)
                        {
                            var filteredRecords = filteredRecordsGender.Where(rec => rec.Distance == swimEvent.Distance && rec.Stroke == swimEvent.Stroke);
                            filteredRecords = filteredRecords.OrderBy(rec => rec.Date).ToList();

                            worksheet.Range[$"A{rowNumber}"].Text = $"{(int)swimEvent.Distance} {SwimmerUtils.StrokeToDutchString(swimEvent.Stroke)}";

                            foreach (var record in filteredRecords)
                            {
                                if (Math.Abs(filteredRecords.Min(rec => rec.Time) - filteredRecords.Last().Time) > 0.01)
                                {
                                    Console.WriteLine($"ERROR: {worksheetName} {swimEvent.Distance}{swimEvent.Stroke}: {record.Time} time is not best time");
                                }
                                for (int i = 0; i < filteredRecords.Count() - 1; i++)
                                {
                                    if (filteredRecords.ToList()[i].Time <= filteredRecords.ToList()[i + 1].Time)
                                    {
                                        Console.WriteLine($"ERROR: {worksheetName} {(int)swimEvent.Distance}{SwimmerUtils.StrokeToDutchString(swimEvent.Stroke)}: {record.Time} time is not in right order");

                                    }
                                }

                                worksheet.Range[$"B{rowNumber}"].Text = SwimmerUtils.ConvertDoubleToTimeString(record.Time);
                                worksheet.Range[$"C{rowNumber}"].Text = record.Name;
                                worksheet.Range[$"D{rowNumber}"].DateTimeValue = record.Date;

                                worksheet.Columns[0].ColumnWidth = 15;
                                worksheet.Columns[1].ColumnWidth = 15;
                                worksheet.Columns[2].ColumnWidth = 15;
                                worksheet.Columns[3].ColumnWidth = 15;
                                rowNumber++;
                            }

                            rowNumber += !filteredRecords.Any() ? 2 : 1;
                        }

                    }
                }
            }


            var now = DateTime.Now;
            workbook.SaveToFile($"{now.Year}-{now.Month}-{now.Day}=Clubrecords history.xlsx", ExcelVersion.Version2013);
            workbook.Worksheets.RemoveAt(workbook.Worksheets.Count - 1);
        }


        // uses the history file to create a record file
        public async Task CreateRecordFileFromHistory()
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Clear();

            var historyRecords = await GetClubRecordsHistoryFromFile();

            List<int> ages = Constants.Individual.AgeGroups;

            Course[] courses = { Course.Short, Course.Long };
            Gender[] genders = { Gender.Female, Gender.Male };

            foreach (int age in ages)
            {
                var filteredRecordsAge = historyRecords.Where(rec => rec.AgeGroup == age);

                var ageGroup = $"{age}-{age + 4}";
                foreach (Course course in courses)
                {
                    var filteredRecordsCourse = filteredRecordsAge.Where(rec => rec.Course == course);
                    foreach (Gender gender in genders)
                    {
                        var filteredRecordsGender = filteredRecordsCourse.Where(rec => rec.Gender == gender);

                        var worksheetName = $"{age}-{age + 4} {(course == Course.Short ? "kb" : "lb")} {(gender == Gender.Female ? "dames" : "heren")}";
                        Worksheet worksheet = workbook.CreateEmptySheet(worksheetName);
                        worksheet.Range["A1"].Text = worksheetName;
                        worksheet.Range["A1"].Style.Font.IsBold = true;


                        var rowNumber = 3;

                        var swimEvents = course == Course.Short ? Constants.Individual.AllSwimEventsShortCourse : Constants.Individual.AllSwimEventsLongCourse;
                        foreach (SwimEvent swimEvent in swimEvents)
                        {
                            worksheet.Range[$"A{rowNumber}"].Text = $"{(int)swimEvent.Distance} {SwimmerUtils.StrokeToDutchString(swimEvent.Stroke)}";

                            var recordsForEvent = filteredRecordsGender?.Where(rec => rec.Distance == swimEvent.Distance && rec.Stroke == swimEvent.Stroke);
                            if (!recordsForEvent.Any())
                            {
                                rowNumber += 2;
                                continue;
                            };
                            var record = recordsForEvent?.OrderBy(rec => rec.Date)?.Last();

                            worksheet.Range[$"B{rowNumber}"].Text = SwimmerUtils.ConvertDoubleToTimeString(record.Time);
                            worksheet.Range[$"C{rowNumber}"].Text = record.Name;
                            worksheet.Range[$"D{rowNumber}"].DateTimeValue = record.Date;

                            worksheet.Columns[0].ColumnWidth = 15;
                            worksheet.Columns[1].ColumnWidth = 15;
                            worksheet.Columns[2].ColumnWidth = 15;
                            worksheet.Columns[3].ColumnWidth = 15;
                            rowNumber += 2;

                        }

                    }
                }

            }


            var now = DateTime.Now;
            //workbook.Worksheets.RemoveAt(workbook.Worksheets.Count - 1);
            workbook.SaveToFile($"{now.Year}-{now.Month}-{now.Day}=Clubrecords actueel.xlsx", ExcelVersion.Version2013);
        }

        public List<ClubRecord> GetRelayHistoryFromFile()
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile("2024-11-25=Clubrecords historie.xlsx");

            var genders = new Gender[]{Gender.Female, Gender.Male, Gender.Mix};
            var courses = new Course[] {Course.Long, Course.Short};
            var records = new List<ClubRecord>();

            foreach (var gender in genders)
            {
                foreach (var course in courses)
                {
                    var genderString = gender == Gender.Female ? "dames" : gender == Gender.Male ? "heren" : "MIX";
                    var worksheetName = $"Estafette {SwimmerUtils.CourseToDutchShorthand(course).ToLower()} {genderString}";
                    var sheet = workbook.Worksheets[worksheetName];

                    var relayStored = "";
                    var ageStored = "";

                    for (int i = 2; i < sheet.Rows.Length; i++)
                    {
                        var swimEvent = sheet.Rows[i]?.Columns[0].Value;
                        if (swimEvent != "")
                        {
                            relayStored = swimEvent;
                        }
                        var ageGroup = sheet.Rows[i].Columns[1].Value;
                        if (ageGroup != "")
                        {
                            ageStored = ageGroup;
                        }
                        var time = sheet.Rows[i].Columns[2].Value;
                        if(time == "") continue;
                        var names = sheet.Rows[i].Columns[3].Value;
                        var date = sheet.Rows[i].Columns[4].Value;
                        

                        records.Add(new ClubRecord
                        {
                            AgeGroup = int.Parse(ageStored.Split("+")[0]),
                            Course = course,
                            Date = SwimmerUtils.GetDateFromDateStringShort(date),
                            Distance = (Distance)(int.Parse(relayStored.Split("x")[1].Trim().Split(" ")[0]) * 4),
                            Gender = gender,
                            IsRelay = true,
                            Name = names,
                            Time = SwimmerUtils.GetTimeFromTimeString(time),
                            Stroke = SwimmerUtils.GetStrokeFromRelayString(relayStored)
                        });

                    }
                }
            }

            var x = 10 + 5;
            return records;
        }

        public void WriteRelaysHistoryIntoExcel(List<ClubRecord> relayRecords)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile("2024-11-7=Clubrecords history.xlsx");

            var genderAndCourseGroups = relayRecords.GroupBy(r => new { r.Gender, r.Course });
            foreach (var genderAgeGroup in genderAndCourseGroups)
            {
                var gender = genderAgeGroup.FirstOrDefault().Gender;
                var course = genderAgeGroup.First().Course;
                var genderString = gender == Gender.Female ? "dames" : gender == Gender.Male ? "heren" : "MIX";
                var worksheetName = $"Estafette {SwimmerUtils.CourseToDutchShorthand(course).ToLower()} {genderString}";

                Worksheet newWorksheet = workbook.CreateEmptySheet(worksheetName);
                newWorksheet.Range["A1"].Text = worksheetName;
                newWorksheet.Range["A1"].Style.Font.IsBold = true;

                var rowNumber = 3;

                var strokeGroups = genderAgeGroup.GroupBy(r => new { r.Stroke, r.Distance });

                foreach (var group in strokeGroups)
                {
                    newWorksheet.Range[$"A{rowNumber}"].Text =
                        SwimmerUtils.RelayDistanceAndStrokeToDutchString(group.FirstOrDefault().Distance, group.FirstOrDefault().Stroke);
                    var ageGroups = group.GroupBy(r => r.AgeGroup);
                    foreach (var ageGroup in ageGroups)
                    {
                        newWorksheet.Range[$"B{rowNumber}"].Text = $"{ageGroup.FirstOrDefault().AgeGroup}+";

                        foreach (var record in ageGroup)
                        {
                            newWorksheet.Range[$"C{rowNumber}"].Text =
                                SwimmerUtils.ConvertDoubleToTimeString(record.Time);
                            newWorksheet.Range[$"D{rowNumber}"].Text = record.Name;
                            newWorksheet.Range[$"E{rowNumber}"].DateTimeValue = record.Date;

                            newWorksheet.Columns[0].ColumnWidth = 15;
                            newWorksheet.Columns[1].ColumnWidth = 10;
                            newWorksheet.Columns[2].ColumnWidth = 10;
                            newWorksheet.Columns[3].ColumnWidth = 60;
                            newWorksheet.Columns[4].ColumnWidth = 15;

                            rowNumber++;
                        }

                        rowNumber++;
                    }

                    rowNumber++;
                }
            }


            var now = DateTime.Now;
            workbook.SaveToFile($"TEST {now.Year}-{now.Month}-{now.Day}=Clubrecords historie.xlsx", ExcelVersion.Version2013);
        }

        // REMOVE, only one time code
        public void GetRelaysIntoExcel()
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile("Clubrecords history_backup.xls");

            Workbook workbook2 = new Workbook();
            workbook2.LoadFromFile("2024-11-7=Clubrecords history.xlsx");

            int[] sheetIndexes = { 44, 45, 46, 47, 48, 49 };

            var allRecords = new List<ClubRecord>();

            foreach (var sheetIndex in sheetIndexes)
            {
                var records = new List<ClubRecord>();
                var sheet = workbook.Worksheets[sheetIndex];
                var course = sheetIndex % 2 == 0 ? Course.Long : Course.Short;
                var gender = sheetIndex - 44 < 2 ? Gender.Female : sheetIndex - 44 < 4 ? Gender.Male : Gender.Mix;


                for (int i = 0; i < sheet.Rows.Length; i++)
                {
                    var swimEvent = sheet.Rows[i]?.Columns[0].Value;
                    var ageGroup = sheet.Rows[i].Columns[1].Value;
                    var time = sheet.Rows[i].Columns[2].Value;
                    if (swimEvent == "" && ageGroup == "" || time == "") continue;
                    var names = sheet.Rows[i].Columns[3].Value;
                    var date = sheet.Rows[i].Columns[4].Value;
                    var namesCleaned = string.Join(";", names.Split(',').Select(name =>
                    {
                        if (name.Contains("("))
                        {
                            //name.Split(" ");
                            return string.Join(" ", name.Split(" ")[..^1]);

                        }

                        return name;
                    }));

                    records.Add(new ClubRecord
                    {
                        AgeGroup = int.Parse(ageGroup.Split("")[0]),
                        Course = course,
                        Date = SwimmerUtils.GetDateFromDateStringShort(date),
                        Distance = (Distance) (int.Parse(swimEvent.Split("x")[1].Split(" ")[0]) * 4),
                        Gender = gender,
                        IsRelay = true,
                        Name = namesCleaned,
                        Time = SwimmerUtils.GetTimeFromTimeString(time),
                        Stroke = SwimmerUtils.GetStrokeFromStringDutch(swimEvent)
                    });

                }

                var genderString = gender == Gender.Female ? "dames" : gender == Gender.Male ? "heren" : "MIX";
                var worksheetName = $"Estafette {SwimmerUtils.CourseToDutchShorthand(course).ToLower()} {genderString}";
                Worksheet newWorksheet = workbook2.CreateEmptySheet(worksheetName);
                newWorksheet.Range["A1"].Text = worksheetName;
                newWorksheet.Range["A1"].Style.Font.IsBold = true;

                var rowNumber = 3;

                var strokeGroups = records.GroupBy(r => new {r.Stroke, r.Distance});

                foreach (var group in strokeGroups)
                {
                        newWorksheet.Range[$"A{rowNumber}"].Text =
                            SwimmerUtils.RelayDistanceAndStrokeToDutchString(group.FirstOrDefault().Distance, group.FirstOrDefault().Stroke);
                    var ageGroups = group.GroupBy(r => r.AgeGroup);
                    foreach (var ageGroup in ageGroups)
                    {
                        newWorksheet.Range[$"B{rowNumber}"].Text = $"{ageGroup.FirstOrDefault().AgeGroup}+";

                        foreach (var record in ageGroup)
                        {
                            newWorksheet.Range[$"C{rowNumber}"].Text =
                                SwimmerUtils.ConvertDoubleToTimeString(record.Time);
                            newWorksheet.Range[$"D{rowNumber}"].Text = record.Name;
                            newWorksheet.Range[$"E{rowNumber}"].DateTimeValue = record.Date;

                            newWorksheet.Columns[0].ColumnWidth = 15;
                            newWorksheet.Columns[1].ColumnWidth = 10;
                            newWorksheet.Columns[2].ColumnWidth = 10;
                            newWorksheet.Columns[3].ColumnWidth = 60;
                            newWorksheet.Columns[4].ColumnWidth = 15;

                            rowNumber++;
                        }

                        rowNumber++;
                    }

                    rowNumber++;
                }

            }

            var now = DateTime.Now;
            workbook2.SaveToFile($"{now.Year}-{now.Month}-{now.Day}=Clubrecords historie.xlsx", ExcelVersion.Version2013);
        }
    }
}
