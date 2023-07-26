using System.Threading.Tasks;
using HtmlAgilityPack;
using RelayCalculator.Services.Models;
using RelayCalculator.Services.Enums;

namespace RelayCalculator.Services.Interfaces
{
    public interface ISwimTimeService
    {
        Task<HtmlDocument> GetSwimmerPageById(int swimmerId);

        Task<CourseTimes> SelectTimesByCourse(int swimmerId, int year, Course course, int? numberOfYearsBackIfNoResult);

        HtmlNodeCollection GetTimeNodes(HtmlDocument doc, Course course);

        double GetBestTime(HtmlNodeCollection table, int sinceYear, int? numberOfYearsBackIfNoResult);

        double ConvertTimeStringToDouble(string time);
    }
}
