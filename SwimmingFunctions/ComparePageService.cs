using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using HtmlAgilityPack;

namespace SwimmingFunctions
{
    public class ComparePageService : IComparePageService
    {
        private static TableClient _tableClient;
        public ComparePageService()
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=relaycalculatorstorage;AccountKey=RPHQsrFZbfUbRtwMqnk1OOZDG5mQBXVPaakvVm5U7O1uKZNG/PPaYFxbTG6wtKindAUbOI+ZVUIH+ASt507mXg==;EndpointSuffix=core.windows.net";
            var tableName = "comparedocument";
            _tableClient = new TableClient(connectionString, tableName);
            // Create the table if it doesn't already exist to verify we've successfully authenticated.
            _tableClient.CreateIfNotExists();
        }

        public async Task<bool> GetPageAndCompare(string url, string pageName)
        {
            var doc = await GetHtmlDocumentByUrl(url);
            var lastDoc = await GetFromStorage(pageName);

            await AddToStorage(doc, pageName);

            return doc.ToString() == lastDoc;
        }

        private static async Task AddToStorage(HtmlDocument document, string pageName)
        {
            var entity = new StoreDocumentEntity(document, pageName);
            await _tableClient.UpsertEntityAsync(entity);
        }

        private static async Task<string> GetFromStorage(string pageName)
        {
            var recordsAsync = await _tableClient.GetEntityAsync<StoreDocumentEntity>(pageName, "last");
            return recordsAsync.Value.Document;
        }

        private static async Task<HtmlDocument> GetHtmlDocumentByUrl(string url)
        {
            url = url.Replace("&amp;", "&");
            var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(response);
            return htmlDocument;
        }
    }

    public class StoreDocumentEntity : ITableEntity
    {
        public StoreDocumentEntity() { }
        public StoreDocumentEntity(HtmlDocument document, string pageName)
        {
            PartitionKey = $"{pageName}";
            RowKey = $"last";
            Document = document.ToString();
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Document { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }


}
