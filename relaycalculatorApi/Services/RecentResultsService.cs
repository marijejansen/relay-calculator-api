using HtmlAgilityPack;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Models;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace RelayCalculator.Api.Services
{
    public class RecentResultsService : IRecentResultsService
    {
        private readonly IHtmlDocumentService _htmlDocumentService;
        private readonly IClubRecordService _clubRecordService;
        private readonly List<int> _meetIds2015to2019 = new List<int> { 589407, 589413, 589556, 589603, 590117, 591251, 591914, 592076, 592347, 592705, 563863, 569763, 593384, 593458, 593556, 594470, 594053,
            594224, 594709, 595551, 595662, 595794, 596206, 596069, 596075, 596103, 596025, 596190, 596208, 596370, 596431, 596588, 597217, 597446, 598092, 598452, 598875, 599473, 600222, 601186, 601545, 601979,
            548413, 602611, 602743, 602913, 603262, 603265, 603091, 603214, 603312, 603406, 603403, 603585, 603823, 603990, 604277, 604235, 604686, 605250, 605817, 606074, 607759, 608060, 608623, 609334,
            609785, 609918, 610071, 609871, 610317, 610350, 610369, 610481, 611090, 611153, 611770, 612225, 612333, 613274, 612831, 613849, 614443, 614837, 615014, 615489, 615694, 615247, 616234, 616839,
            615823, 615980, 616614, 616500, 615534, 616820, 616816, 616831, 616864, 617023, 617887, 617903, 618072, 618844, 619926, 619990};
        private readonly List<int> _meetIds2014to2019international = new List<int> { 617723, 610321, 610871, 613162, 603443, 604260, 604944, 595984, 597524, 597895, 590570, 582586,};
        private readonly List<int> _meetIds2020to2022 = new List<int> { 620148, 620753, 621149, 621320, 621332, 621663, 622911, 626693, 626698, 627058, 627054, 627824, 630372, 630540, 630603, 631305, 631182,
            631899, 632920, 632118, 632485, 632563, 632584, 632927, 633095, 633104, 633025, 633542, 633737, 633353, 634432, 634877 };
        private readonly List<int> _meetsIds2023to2024sep = new List<int> { 635237, 636259, 636671, 636794, 637130, 638215, 637828, 638877, 638502, 639052, 639545, 639808, 639850, 639913, 639997, 640773, 640334,
            640993, 641732, 641679, 642135, 642434, 642655, 643158, 643363, 643808, 643722, 644122, 644297, 644594, 645715, 645244, 646171, 646354, 646494, 646317, 647047, 647049, 647060, 646752, 647149, 647471, 647504, 647480};

        public RecentResultsService(IHtmlDocumentService htmlDocumentService, IClubRecordService clubRecordService)
        {
            _htmlDocumentService = htmlDocumentService;
            _clubRecordService = clubRecordService;
        }

        public async Task<IEnumerable<ClubRecord>> GetNewRecordsFromSwimrankings(DateTime? fromDate, bool fromList = false)
        {

            var allNewRecords = new List<ClubRecord>();

            if (fromList)
            {
                var allMeets = _meetIds2015to2019.Concat(_meetIds2014to2019international).Concat(_meetIds2020to2022).Concat(_meetsIds2023to2024sep);

                foreach (var meetId in allMeets)
                {
                    var meetUrl = "?page=meetDetail&meetId=" + meetId;
                    var meetDocument = await GetClubMeetDocument(meetUrl);

                    if (IsClubMeetAndMasters(meetDocument))
                    {
                        var newRecords = await GetRecordsFromMeet(meetDocument);
                        allNewRecords.AddRange(newRecords);
                    }
                }
            }
            else
            {
                do
                {
                    var htmlDocument = await _htmlDocumentService.GetRecentMeetsPage(fromDate);

                    var trsWithMeets = htmlDocument.DocumentNode.Descendants("tr").Where(node => node.GetAttributeValue("class", "").Contains("meetSearch0") || node.GetAttributeValue("class", "").Contains("meetSearch1"));
                    var sortedMeets = trsWithMeets.Reverse();

                    foreach (var meet in sortedMeets)
                    {
                        var meetUrl = GetMeetUrl(meet);
                        HtmlDocument meetDocument = new HtmlDocument();
                        try
                        {
                            meetDocument = await GetClubMeetDocument(meetUrl);
                        }
                        catch
                        {
                            Console.WriteLine("can not open meet ", GetMeetId(meetUrl));
                            continue;
                        }

                        if (IsClubMeetAndMasters(meetDocument))
                        {
                            //Console.WriteLine(GetMeetId(meetUrl));
                            var newRecords = await GetRecordsFromMeet(meetDocument);
                            allNewRecords.AddRange(newRecords);
                        }
                    }

                    fromDate = fromDate?.AddMonths(1);
                }  while (fromDate != null && fromDate < DateTime.Now);
            }


            return allNewRecords;
        }




        private async Task<List<ClubRecord>> GetRecordsFromMeet(HtmlDocument meetDocument)
        {
            var title = meetDocument.DocumentNode.Descendants("td").First(node => node.GetAttributeValue("class", "").Contains("titleLeft")).InnerText;
            var date = meetDocument.DocumentNode.Descendants("td").Last(node => node.GetAttributeValue("class", "").Contains("titleRight")).InnerText.Replace("&nbsp;", " ");
            var course = meetDocument.DocumentNode.Descendants("td").First(node => node.GetAttributeValue("class", "").Contains("titleRight")).InnerText;

            Console.WriteLine($"{date} - {title}");

            var trs = meetDocument.DocumentNode.Descendants("table").First(t => t.HasClass("meetResult")).Descendants("tr");
            //var raceResults = meetDocument.DocumentNode.Descendants("tr").Where(node => node.GetAttributeValue("class", "").Contains("meetResult1") || node.GetAttributeValue("class", "").Contains("meetResult0"));

            var swimmerMeetResult = new SwimmerMeetResult { Course = SwimmerUtils.GetCourseFromString(course), Date = SwimmerUtils.GetDateFromDateString(date) };

            var allNewRecords = new List<ClubRecord>();


            foreach (var item in trs)
            {
                if (item.GetAttributeValue("class", "").Contains("meetResultHead"))
                {
                    swimmerMeetResult = new SwimmerMeetResult { Course = SwimmerUtils.GetCourseFromString(course), Date = SwimmerUtils.GetDateFromDateString(date) };
                    var genderText = item.Descendants("th").FirstOrDefault()?.InnerText;
                    swimmerMeetResult.Gender = SwimmerUtils.GetGenderFromString(genderText);
                    continue;
                }

                var nameNode = item.Descendants("td")?.FirstOrDefault(t => t.HasClass("nameImportant"));
                if (nameNode != null)
                {
                    var nameArray = SwimmerUtils.GetNameArrayFromString(nameNode.Descendants("a").First().InnerText);
                    swimmerMeetResult.FirstName = nameArray[1];
                    swimmerMeetResult.LastName = nameArray[0];
                    try
                    {
                        swimmerMeetResult.BirthYear = int.Parse(nameNode.InnerText.Split("- ").Last());
                    }
                    catch (Exception)
                    {
                        var acs = 1;
                    }
                    continue;
                }

                var tds = item.Descendants("td").ToList();
                if (tds?.FirstOrDefault(t => t.HasClass("name")) != null)
                {
                    var eventText = tds.First(t => t.HasClass("name")).InnerText;
                    if (eventText.Contains("Lap")) continue;

                    swimmerMeetResult.EventResults = new EventResult[] { };
                    swimmerMeetResult.EventResults = swimmerMeetResult.EventResults.Append(new EventResult()
                    {
                        SwimEvent = SwimmerUtils.GetEventFromString(eventText),
                        Time = SwimmerUtils.ConvertTimeStringToDouble(tds.First(t => t.HasClass("swimtime"))
                            .InnerText)
                    });
                    var newRecords = await _clubRecordService.GetNewRecordsFromMeetResults(swimmerMeetResult);
                    if (newRecords.Any())
                    {
                        allNewRecords.AddRange(newRecords);
                    }
                }
            }
            return allNewRecords;
        }




        private async Task<HtmlDocument> GetClubMeetDocument(string meetUrl)
        {
            //var meetUrl = "?page=meetDetail&meetId=640334";
            var clubIdQueryString = "&clubId=89878";
            // get the club meet results, returns a page without results if no starts from club 
            return await _htmlDocumentService.GetHtmlDocumentByRelativeUrl(meetUrl + clubIdQueryString);
        }

        private string GetMeetUrl(HtmlNode meetNode)
        {
            return meetNode.Descendants("td").FirstOrDefault(n => n.HasClass("city"))?.Descendants("a").First().GetAttributeValue("href", "");
        }

        private string GetMeetId(string meetUrl)
        {
            var match = Regex.Match(meetUrl, @"meetId=(\d+)");
            return match.Success ? match.Groups[1].Value : "";
        }

        private bool IsClubMeetAndMasters(HtmlDocument meetDocument)
        {
            var firstTrResult = meetDocument?.DocumentNode.Descendants("table")?.FirstOrDefault()
                ?.Descendants("tr")?
                .Where(node => node.GetAttributeValue("class", "").Contains("meetResult0") || node.GetAttributeValue("class", "")
                    .Contains("meetResult1")).FirstOrDefault(r => r.Descendants("td").Any(td => td.GetAttributeValue("class", "").Contains("swimtime")));
            var lastTrResult = meetDocument?.DocumentNode.Descendants("table")?.FirstOrDefault()
                ?.Descendants("tr")?
                .Where(node => node.GetAttributeValue("class", "").Contains("meetResult0") || node.GetAttributeValue("class", "")
                    .Contains("meetResult1")).LastOrDefault(r => r.Descendants("td").Any(td => td.GetAttributeValue("class", "").Contains("swimtime")));
            if (firstTrResult == null || lastTrResult == null) return false;
            return (firstTrResult.Descendants("td")
                       ?.FirstOrDefault(td => td.GetAttributeValue("class", "").Contains("swimtime"))?.Descendants("a")
                       .First().InnerHtml.Contains("<sup>M</sup>") ?? false)
                   || (lastTrResult.Descendants("td")
                       ?.FirstOrDefault(td => td.GetAttributeValue("class", "").Contains("swimtime"))?.Descendants("a")
                       .First().InnerHtml.Contains("<sup>M</sup>") ?? false);
        }


    }
}
