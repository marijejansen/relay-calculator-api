using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using RelayCalculator.Api.Services.Interfaces;

namespace RelayCalculator.Api.Services
{
    public class HtmlDocumentService : IHtmlDocumentService
    {
        public async Task<HtmlDocument> GetHtmlPerStroke(int id, int style)
        {
            var url = $"https://www.swimrankings.net/index.php?page=athleteDetail&athleteId={id}&styleId={style}";
            
            return await GetHtmlDocumentByUrl(url);
        }

        public async Task<HtmlDocument> GetSearchResultsPage(string firstName, string lastName)
        {
            var url = $"https://www.swimrankings.net/index.php?&internalRequest=athleteFind&athlete_clubId=-1&athlete_gender=-1&athlete_lastname={lastName}&athlete_firstname={firstName}";
            
            return await GetHtmlDocumentByUrl(url);
        }

        public async Task<HtmlDocument> GetRecentMeetsPage()
        {
            var url = $"https://www.swimrankings.net/index.php?page=meetSelect&selectPage=RECENT&meetType=1&nationId=273";

            return await GetHtmlDocumentByUrl(url);
        }

        public async Task<HtmlDocument> GetRecentMeetsPage(DateTime? fromDate)
        {
            var selectArg = fromDate != null ? fromDate?.Year + "_m" + fromDate?.Month : "RECENT";

            // nederlands
            //var url = $"https://www.swimrankings.net/index.php?page=meetSelect&meetType=1&nationId=273&selectPage={selectArg}";
            //world wide
            var url = $"https://www.swimrankings.net/index.php?page=meetSelect&meetType=1&selectPage={selectArg}";
            return await GetHtmlDocumentByUrl(url);
        }

        public async Task<HtmlDocument> GetHtmlDocumentByRelativeUrl(string url)
        {
            return await this.GetHtmlDocumentByUrl($"https://www.swimrankings.net/index.php{url}");
        }

        public async Task<HtmlDocument> GetHtmlDocumentByUrl(string url)
        {
            url = url.Replace("&amp;", "&");
            var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);
            return htmlDocument;
        }
    }
}
