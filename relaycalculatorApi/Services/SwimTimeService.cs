﻿using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;
using RelayCalculator.Api.Utils;

namespace RelayCalculator.Api.Services
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

        public async Task<CourseTimes> SelectTimesByCourse(int swimmerId, int year, Course course, int? numberOfYearsBackIfNoResult, bool? getAllTimes)
        {
            CourseTimes times = new CourseTimes();
            var strokes = getAllTimes != null && (bool)getAllTimes ? Constants.SwimRankingsPage.AllStrokes : Constants.SwimRankingsPage.StrokesForRelays;
            foreach (var stroke in strokes)
            {
                HtmlDocument doc = await _htmlDocumentService.GetHtmlPerStroke(swimmerId, stroke.Value);
                HtmlNodeCollection table = GetTimeNodes(doc, course);

                var bestTime = GetBestTime(table, year, numberOfYearsBackIfNoResult);

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
        public double GetBestTime(HtmlNodeCollection table, int sinceYear, int? numberOfYearsBackIfNoResult)
        {
            double bestTime = 0;
            double backUpTime = 0;
            try
            {
                foreach (var tr in table)
                {
                    //get the date
                    var tdDate = tr.SelectSingleNode(".//td[@class='date']");
                    var date = tdDate.InnerText;
                    var splitDate = date.Split(';');
                    var yearString = splitDate[2];
                    var year = Convert.ToInt32(yearString);

                    //check if the date falls in the right timespan
                    if (year >= sinceYear)
                    {
                        //get the time
                        var stringTime = tr.SelectSingleNode(".//a[@class='time']").InnerText;
                        var time = SwimmerUtils.ConvertTimeStringToDouble(stringTime);

                        //check for the fastest time
                        if (time < bestTime || bestTime <= 0) bestTime = time;

                    } else if (numberOfYearsBackIfNoResult != null && year >= sinceYear - numberOfYearsBackIfNoResult)
                    {
                        //get the time
                        var stringTime = tr.SelectSingleNode(".//a[@class='time']").InnerText;
                        var time = SwimmerUtils.ConvertTimeStringToDouble(stringTime);

                        //check for the fastest backup time
                        if (time < backUpTime || backUpTime <= 0) backUpTime = time;
                    }
                }

                return bestTime != 0 ? Math.Round(bestTime, 2) : Math.Round(backUpTime, 2);
            }
            catch
            {
                return bestTime;
            }
        }
    }
}
