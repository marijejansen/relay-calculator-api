using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RelayCalculator.Services.Models;
using System.Globalization;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Interfaces;
using System.Diagnostics;

namespace RelayCalculator.Services
{
    public class SwimTimeService : ISwimTimeService
    {
        private readonly IHtmlDocumentService _htmlDocumentService;

        public SwimTimeService(IHtmlDocumentService htmlDocumentService)
        {
            _htmlDocumentService = htmlDocumentService;
        }
        public async Task<HtmlDocument> GetSwimmerPageById(int swimmerId)
        {
            var swimmerPage = $"https://www.swimrankings.net/index.php?page=athleteDetail&athleteId={swimmerId}";
            HtmlDocument htmlDoc = await _htmlDocumentService.GetHtmlDocumentByUrl(swimmerPage);
            return htmlDoc;
        }

        public async Task<CourseTimes> SelectTimesByCourse(int swimmerId, int year, Course course)
        {
            CourseTimes times = new CourseTimes();

            foreach (var stroke in Constants.SwimRankingsPage.Strokes)
            {
                HtmlDocument doc = await _htmlDocumentService.GetHtmlPerStroke(swimmerId, stroke.Value);
                HtmlNodeCollection table = GetTimeNodes(doc, course);

                var bestTime = GetBestTime(table, year);

                times.GetType().GetProperty(stroke.Key)?.SetValue(times, bestTime);
            };

            return times;
        }

        //takes in a HtmlDocument 
        //returns the nodes with times on LongCourse as HtmlNodeCollection
        public HtmlNodeCollection GetTimeNodes(HtmlDocument doc, Course course)
        {

            var columns = doc.DocumentNode.Descendants("table").FirstOrDefault(n => n.HasClass("twoColumns"));
            var tables = doc.DocumentNode.SelectNodes("//table[@class='twoColumns']//table");

            if (course == Course.Long)
            {
                return tables[0].SelectNodes(".//tr[@class='athleteRanking0'] | .//tr[@class='athleteRanking1']");
            }

            return tables[1].SelectNodes(".//tr[@class='athleteRanking0'] | .//tr[@class='athleteRanking1']");
        }

        //takes in a HtmlNodeCollection in which times of a specific course are found and a year from which to search
        //returns a double besttime
        public double GetBestTime(HtmlNodeCollection table, int sinceYear)
        {
            double bestTime = 0;
            try
            {
                foreach (var tr in table)
                {
                    //get the date
                    var tdDate = tr.SelectSingleNode(".//td[@class='date']");
                    var date = tdDate.InnerText;
                    var splitDate = date.Split(';');
                    var yearString = splitDate[2];
                    int year = Convert.ToInt32(yearString);

                    //check if the date falls in the right timespan
                    if (year >= sinceYear)
                    {

                        //get the time
                        var stringTime = tr.SelectSingleNode(".//a[@class='time']").InnerText;
                        double time = ConvertTimeStringToDouble(stringTime);

                        //check for the fastest time
                        if (time < bestTime || bestTime <= 0)
                        {
                            bestTime = time;
                        }
                    }
                }

                return Math.Round(bestTime, 2);
            }
            catch
            {
                return bestTime;
            }
        }

        //TODO: ergens anders neerzetten
        public double ConvertTimeStringToDouble(string time)
        {
            double timeInSeconds;
            if (time.Contains(":"))
            {
                var splitted = time.Split(':');
                var minutes = Convert.ToDouble(splitted[0]);
                timeInSeconds = minutes * 60;
                var seconds = Convert.ToDouble(splitted[1], CultureInfo.InvariantCulture);
                timeInSeconds += seconds;
            }
            else
            {
                timeInSeconds = Convert.ToDouble(time, CultureInfo.InvariantCulture);
            }
            return timeInSeconds;
        }
    }
}
