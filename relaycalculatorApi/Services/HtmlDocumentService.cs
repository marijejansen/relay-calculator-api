using HtmlAgilityPack;
using RelayCalculator.Api.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:142.0) Gecko/20100101 Firefox/142.0");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br, zstd");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");

            var response = await client.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);
            return htmlDocument;
        }
    }
}
