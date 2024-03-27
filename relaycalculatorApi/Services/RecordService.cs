using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Spire.Xls;
using HtmlAgilityPack;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;
using RelayCalculator.Api.Utils;

namespace RelayCalculator.Api.Services
{
    public class RecordService : IRecordService
    {
        private static IHtmlDocumentService _htmlDocumentService;

        public RecordService(IHtmlDocumentService htmlDocumentService)
        {
            _htmlDocumentService = htmlDocumentService;
        }

        public async Task<List<Record>> GetRelayRecords()
        {
            var file = $"MastersRecords.lef";
            var document = new XmlDocument();
            document.Load(file);
            var recordNodes = document.GetElementsByTagName("RECORDLIST");

            List<XmlNode> recordGroupList = this.GetRecordGroupList(recordNodes, EventType.Relay);
            var relayRecords = new List<Record>();

            foreach (var recordGroup in recordGroupList)
            {
                var course = NodeIdentifiers.GetCourseForRecordGroup(recordGroup);
                var gender = NodeIdentifiers.GetGenderForRecordsGroup(recordGroup);
                var type = NodeIdentifiers.GetRecordTypeForRecordsGroup(recordGroup);
                var age = NodeIdentifiers.GetAgeForRecordGroup(recordGroup);

                var records = GetRecordNodesFromGroup(recordGroup);

                foreach (var record in records)
                {
                    var relay = NodeIdentifiers.GetRelayTypeForRecord(record);
                    var time = NodeIdentifiers.GetTimeForRecord(record);

                    relayRecords.Add(new Record
                    {
                        Age = age.GetValueOrDefault(),
                        Course = course.GetValueOrDefault(),
                        Gender = gender.GetValueOrDefault(),
                        RecordType = type.GetValueOrDefault(),
                        RelayType = relay.GetValueOrDefault(),
                        Time = time
                    });
                }
            }

            return relayRecords;
        }

        private async Task<List<Record>> DownloadRecords()
        {
            HtmlDocument doc =
                await _htmlDocumentService.GetHtmlDocumentByUrl(
                    "https://mastersprint.nl/download/#");

            var button = doc.DocumentNode.Descendants("a").Where(node => node.GetAttributeValue("class", "").Contains("download-on-click")).ToList();

            return new List<Record>();
        }

        private List<XmlNode> GetRecordGroupList(XmlNodeList nodeList, EventType eventType)
        {
            var recordGroups = new List<XmlNode>();

            foreach (XmlNode node in nodeList)
            {
                var rightType = eventType == EventType.Relay ? NodeIdentifiers.GetNodeIsRelayRecord(node) : NodeIdentifiers.GetNodeIsIndividualRecord(node);
                if (rightType)
                {
                    recordGroups.Add(node);
                }
            }

            return recordGroups;
        }

        private List<XmlNode> GetRecordNodesFromGroup(XmlNode recordGroup)
        {
            var recordNodes = new List<XmlNode>();

            foreach (XmlNode childNode in recordGroup.ChildNodes)
            {
                if (childNode.Name != "RECORDS")
                {
                    continue;
                }

                foreach (XmlNode recordNode in childNode.ChildNodes)
                {
                    if (recordNode.Name == "RECORD")
                    {
                        recordNodes.Add(recordNode);
                    }
                }
            }

            return recordNodes;
        }
    }
}
