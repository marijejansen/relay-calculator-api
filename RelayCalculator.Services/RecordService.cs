using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services
{
    public class RecordService : IRecordService
    {
        private static IHtmlDocumentService _htmlDocumentService;

        public RecordService(IHtmlDocumentService htmlDocumentService)
        {
            _htmlDocumentService = htmlDocumentService;
        }

        public async Task<List<Record>> GetRecords()
        {
            HtmlDocument doc =
                await _htmlDocumentService.GetHtmlDocumentByUrl(
                    "https://mastersprint.nl/download/#");

            var button = doc.DocumentNode.Descendants("a").Where(node => node.GetAttributeValue("class", "").Contains("download-on-click")).ToList();

            return new List<Record>();
        }
    }
}
