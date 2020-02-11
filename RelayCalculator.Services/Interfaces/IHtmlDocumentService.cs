using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RelayCalculator.Services.Interfaces
{
    public interface IHtmlDocumentService
    {
        Task<HtmlDocument> GetHtmlPerStroke(int id, int style);

        Task<HtmlDocument> GetSearchResultsPage(string firstName, string lastName);

        Task<HtmlDocument> GetHtmlDocumentByUrl(string url);
    }
}
