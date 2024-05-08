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

namespace RelayCalculator.Api.Services
{
    public class RecentResultsService : IRecentResultsService
    {
       private readonly IHtmlDocumentService _htmlDocumentService;
        private readonly IClubRecordService _clubRecordService;

        public RecentResultsService(IHtmlDocumentService htmlDocumentService, IClubRecordService clubRecordService)
        {
            _htmlDocumentService = htmlDocumentService;
            _clubRecordService = clubRecordService;
        }

        public async Task<IEnumerable<ClubRecord>> GetNewRecordsFromSwimrankings(DateTime? fromDate)
        {

            var allNewRecords = new List<ClubRecord>();

            do
            {
                var htmlDocument = await _htmlDocumentService.GetRecentMeetsPage(fromDate);

                var trsWithMeets = htmlDocument.DocumentNode.Descendants("tr").Where(node => node.GetAttributeValue("class", "").Contains("meetSearch0") || node.GetAttributeValue("class", "").Contains("meetSearch1"));
                var sortedMeets = trsWithMeets.Reverse();

                foreach (var meet in sortedMeets)
                {
                    var meetDocument = await GetClubMeetDocument(meet);

                    if (IsClubMeetAndMasters(meetDocument))
                    {
                        var title = meetDocument.DocumentNode.Descendants("td").First(node => node.GetAttributeValue("class", "").Contains("titleLeft")).InnerText;
                        var date = meetDocument.DocumentNode.Descendants("td").Last(node => node.GetAttributeValue("class", "").Contains("titleRight")).InnerText.Replace("&nbsp;", " ");
                        var course = meetDocument.DocumentNode.Descendants("td").First(node => node.GetAttributeValue("class", "").Contains("titleRight")).InnerText;
                        
                        Console.WriteLine($"{date} - {title}");

                        var trs = meetDocument.DocumentNode.Descendants("table").First(t => t.HasClass("meetResult")).Descendants("tr");
                        //var raceResults = meetDocument.DocumentNode.Descendants("tr").Where(node => node.GetAttributeValue("class", "").Contains("meetResult1") || node.GetAttributeValue("class", "").Contains("meetResult0"));

                        var swimmerMeetResult = new SwimmerMeetResult{Course = SwimmerUtils.GetCourseFromString(course), Date = SwimmerUtils.GetDateFromDateString(date) };

                        foreach (var item in trs) {
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

                                swimmerMeetResult.EventResults = new EventResult[]{};
                                swimmerMeetResult.EventResults = swimmerMeetResult.EventResults.Append(new EventResult()
                                {
                                    SwimEvent = SwimmerUtils.GetEventFromString(eventText),
                                    Time = SwimmerUtils.ConvertTimeStringToDouble(tds.First(t => t.HasClass("swimtime"))
                                        .InnerText)
                                });
                                var newRecords = await _clubRecordService.CheckAndGetNewRecords(swimmerMeetResult);
                                if (newRecords.Any())
                                {
                                    var tasks = newRecords.Select(async record =>
                                        {
                                            await _clubRecordService.AddToStorage(record);
                                        }
                                    );
                                    await Task.WhenAll(tasks);
                                    allNewRecords.AddRange(newRecords);
                                }
                            }

                        }
                        
                    }
                }
                
                fromDate = fromDate?.AddMonths(1);
            }
            while (fromDate != null && fromDate < DateTime.Now);

            return allNewRecords;
        }

        private async Task<HtmlDocument> GetClubMeetDocument(HtmlNode meetNode)
        {
            var meetUrl = meetNode.Descendants("td").FirstOrDefault(n => n.HasClass("city"))?.Descendants("a").First().GetAttributeValue("href", "");
            //var meetUrl = "?page=meetDetail&meetId=640334";
            var clubIdQueryString = "&clubId=89878";
            // get the club meet results, returns a page without results if no starts from club 
            return await _htmlDocumentService.GetHtmlDocumentByRelativeUrl(meetUrl + clubIdQueryString);
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
