using HtmlAgilityPack;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RelayCalculator.Api.Services
{
    public class SwimmerStatService : ISwimmerStatService
    {
        private readonly IHtmlDocumentService _htmlDocumentService;
        private readonly ISwimTimeService _swimTimeService;
        private int _swimmerId;

        public SwimmerStatService(IHtmlDocumentService htmlDocumentService, ISwimTimeService swimTimeService)
        {
            _htmlDocumentService = htmlDocumentService;
            _swimTimeService = swimTimeService;
        }

        public async Task<SwimmerStatModel> GetSwimmerStats(int swimmerId, DateTime? startDate = null)
        {
            if (startDate == null)
            {
                startDate = new DateTime(2023, 01, 01);
            }

            _swimmerId = swimmerId;
            var meetsPage = await GetSwimmerMeetPage();
            var meetTable = meetsPage.DocumentNode.Descendants("table").FirstOrDefault(n => n.HasClass("athleteMeet"));

            var meets = meetTable.SelectNodes(".//tr[@class='athleteMeet0'] | .//tr[@class='athleteMeet1']");
            var meetsList = new List<Meet>();

            foreach (var meet in meets)
            {
             
                var dateNode = meet.SelectSingleNode(".//td[@class='date']");
                var date = GetDate(dateNode);

                // TODO: allow for other dates
                if (date < startDate)
                {
                    continue;
                }

                var city = meet.SelectSingleNode(".//td[@class='city']");
                var course = meet.SelectSingleNode(".//td[@class='course']").InnerText == "25m" ? Enums.Course.Short : Enums.Course.Long;


                var meetQueryPar = city.SelectSingleNode("a").Attributes.FirstOrDefault(a => a.Name == "href").Value.Split("&amp;");
                var meetId = meetQueryPar[1].Split("=")[1];
                var clubId = meetQueryPar[2].Split("=")[1];
                var meetPage = await GetMeetPage(meetId, clubId);
                var meetModel = new Meet() { Course = course, Date = date, MeetId = int.Parse(meetId) }; 

                meetModel.RacePerformances = GetRacePerformanceForMeet(meetPage);
                meetsList.Add(meetModel);
            }

            return new SwimmerStatModel() { SwimmerId = swimmerId, Meets = meetsList.AsEnumerable<Meet>()};
        }

        private async Task<HtmlDocument> GetSwimmerMeetPage()
        {
            var swimmerPage = $"https://www.swimrankings.net/index.php?page=athleteDetail&athletePage=MEET&athleteId={_swimmerId}";
            HtmlDocument htmlDoc = await _htmlDocumentService.GetHtmlDocumentByUrl(swimmerPage);
            return htmlDoc;
        }

        private async Task<HtmlDocument> GetMeetPage(string meetId, string clubId)
        {
            var swimmerPage = $"https://www.swimrankings.net/index.php?page=meetDetail&meetId={meetId}&clubId={clubId}";
            HtmlDocument htmlDoc = await _htmlDocumentService.GetHtmlDocumentByUrl(swimmerPage);
            return htmlDoc;
        }

        private RacePerformance[] GetRacePerformanceForMeet(HtmlDocument meetPage)
        {
            var performancesForMeet = new List<RacePerformance>();
            var trs = meetPage.DocumentNode.Descendants("tr");
            var foundSwimmer = false;
            foreach (var tr in trs)
            {
                if (foundSwimmer && tr.HasClass("meetResult0"))
                {
                    var performance = GetSingleRacePerformance(tr);
                    if(performance != null)
                    {
                        performancesForMeet.Add(performance);
                    }
                }

                else if (tr.HasClass("meetResult1"))
                {
                    var aNode = tr.SelectSingleNode(".//td[@class='nameImportant']").SelectSingleNode("a");
                    var href = aNode.Attributes["href"].Value;
                    var swimmerId = int.Parse(href.Split('&').Last().Split("=")[1]);
                    if (swimmerId == _swimmerId)
                    {
                        foundSwimmer = true;
                    }
                    else if (foundSwimmer)
                    {
                        break;
                    }
                }
            }
            return performancesForMeet.ToArray();
        }

        private RacePerformance GetSingleRacePerformance(HtmlNode node)
        {
            var aNode = node.SelectSingleNode(".//td[@class='name']").SelectSingleNode("a");
            var href = aNode.Attributes["href"].Value;
            var styleId = int.Parse(href.Split('&').Last().Split("=")[1]);

            var modelWithDistanceAndStroke = GetRacePerformanceWithDistance(styleId);
            if(modelWithDistanceAndStroke == null) { return null; };

            var swimTime = _swimTimeService.ConvertTimeStringToDouble(node.SelectSingleNode(".//td[@class='swimtime']").SelectSingleNode("a").InnerText);
            var percentageNode = node.SelectNodes(".//td[@class='date']");
            var percentageString = percentageNode != null ? percentageNode.First()?.SelectSingleNode("i")?.InnerText.Replace("%", "") : "0.00";

            double.TryParse(percentageString, NumberStyles.Any, CultureInfo.InvariantCulture, out var percentage);
            
            return new RacePerformance()
            {
                Distance = modelWithDistanceAndStroke.Distance,
                Stroke = modelWithDistanceAndStroke.Stroke,
                Time = swimTime,
                Percentage = percentage
            };
        }

        private RacePerformance GetRacePerformanceWithDistance(int styleId)
        {
            switch (styleId)
            {
                case 1:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.Fifty,
                        Stroke = Enums.Stroke.Freestyle,
                    };
                case 2:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.Hundred,
                        Stroke = Enums.Stroke.Freestyle,
                    };
                case 3:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.TwoHundred,
                        Stroke = Enums.Stroke.Freestyle,
                    };
                case 5:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.FourHundred,
                        Stroke = Enums.Stroke.Freestyle,
                    };
                case 6:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.EightHundred,
                        Stroke = Enums.Stroke.Freestyle,
                    };
                case 8:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.FifteenHundred,
                        Stroke = Enums.Stroke.Freestyle,
                    };
                case 9:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.Fifty,
                        Stroke = Enums.Stroke.Backstroke,
                    };
                case 10:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.Hundred,
                        Stroke = Enums.Stroke.Backstroke,
                    };
                case 11:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.TwoHundred,
                        Stroke = Enums.Stroke.Backstroke,
                    };
                case 12:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.Fifty,
                        Stroke = Enums.Stroke.Breaststroke,
                    };
                case 13:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.Hundred,
                        Stroke = Enums.Stroke.Breaststroke,
                    };
                case 14:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.TwoHundred,
                        Stroke = Enums.Stroke.Breaststroke,
                    };
                case 15:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.Fifty,
                        Stroke = Enums.Stroke.Butterfly,
                    };
                case 16:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.Hundred,
                        Stroke = Enums.Stroke.Butterfly,
                    };
                case 17:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.TwoHundred,
                        Stroke = Enums.Stroke.Breaststroke,
                    };
                case 18:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.TwoHundred,
                        Stroke = Enums.Stroke.Medley,
                    };
                case 19:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.FourHundred,
                        Stroke = Enums.Stroke.Medley,
                    };
                case 20:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.Hundred,
                        Stroke = Enums.Stroke.Medley,
                    };
                case 47:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.TwentyFive,
                        Stroke = Enums.Stroke.Freestyle,
                    };
                case 48:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.TwentyFive,
                        Stroke = Enums.Stroke.Backstroke,
                    };
                case 49:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.TwentyFive,
                        Stroke = Enums.Stroke.Breaststroke,
                    };
                case 50:
                    return new RacePerformance()
                    {
                        Distance = Enums.Distance.TwentyFive,
                        Stroke = Enums.Stroke.Butterfly,
                    };
                default: return null;
            }
        }


        private DateTime GetDate(HtmlNode dateNode)
        {
            var date = dateNode.InnerText;
            var splitDate = date.Split("&nbsp;");
            var yearString = splitDate.Last();
            var dayString = splitDate.First();
            var monthString = splitDate[splitDate.Length - 2];
            var year = Convert.ToInt32(yearString);

            return new DateTime(year, GetMonth(monthString), int.Parse(dayString));
        }

        private int GetMonth(string month)
        {
            switch (month.ToLower())
            {
                case "jan":
                    return 1;
                case "feb":
                    return 2;
                case "mar":
                    return 3;
                case "apr":
                    return 4;
                case "may":
                case "mei":
                    return 5;
                case "jun":
                    return 6;
                case "jul":
                    return 7;
                case "aug":
                    return 8;
                case "sep":
                    return 9;
                case "oct":
                case "okt":
                    return 10;
                case "nov":
                    return 11;
                case "dec":
                    return 12;
                default:
                    return 0;
            }
        }
    }
}
