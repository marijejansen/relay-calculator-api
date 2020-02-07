using System.Threading.Tasks;
using HtmlAgilityPack;
using RelayCalculator.Services.Models;
using RelayCalculator.Services.Enums;

namespace RelayCalculator.Services.Interfaces
{
    public interface ICrawlSwimTimeService
    {
        Task<HtmlDocument> GetSwimmerPageById(int swimmerId);

        Task<CourseTimes> SelectTimesByCourse(int swimmerId, int year, Course course);

        HtmlNodeCollection GetTimeNodes(HtmlDocument doc, Course course);

        double GetBestTime(HtmlNodeCollection table, int sinceYear);

        Task<HtmlDocument> GetHtmlPerStroke(int id, int style);

        Task<HtmlDocument> GetHtmlDocumentByUrl(string url);

        double ConvertTimeStringToDouble(string time);
    }
}
