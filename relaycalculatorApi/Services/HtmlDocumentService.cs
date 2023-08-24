using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RelayCalculator.Services.Interfaces;

namespace RelayCalculator.Services
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

        public async Task<HtmlDocument> GetHtmlDocumentByUrl(string url)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);
            return htmlDocument;
        }
    }
}
