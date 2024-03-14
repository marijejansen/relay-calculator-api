using System.Threading.Tasks;
using HtmlAgilityPack;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface ISwimTimeService
    {
        Task<HtmlDocument> GetSwimmerPageById(int swimmerId);

        Task<CourseTimes> SelectTimesByCourse(int swimmerId, int year, Course course, int? numberOfYearsBackIfNoResult, bool? getAllTimes);

        HtmlNodeCollection GetTimeNodes(HtmlDocument doc, Course course);

        double GetBestTime(HtmlNodeCollection table, int sinceYear, int? numberOfYearsBackIfNoResult);
    }
}
