using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RelayCalculator.Api.Services.Interfaces
{
    public interface IHtmlDocumentService
    {
        Task<HtmlDocument> GetHtmlPerStroke(int id, int style);

        Task<HtmlDocument> GetSearchResultsPage(string firstName, string lastName);

        Task<HtmlDocument> GetHtmlDocumentByUrl(string url);

        Task<HtmlDocument> GetHtmlDocumentByRelativeUrl(string url);

        Task<HtmlDocument> GetRecentMeetsPage(DateTime? fromDateTime);
    }
}
