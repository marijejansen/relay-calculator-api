using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Services
{
    public class SearchSwimmersService : ISearchSwimmerService
    {
        private static IHtmlDocumentService _htmlDocumentService;

        public SearchSwimmersService(IHtmlDocumentService htmlDocumentService)
        {
            _htmlDocumentService = htmlDocumentService;
        }

        public async Task<List<Swimmer>> FindSwimmersByName(string firstName, string lastName)
        {
            var htmlDocument = await _htmlDocumentService.GetSearchResultsPage(firstName, lastName);

            var trs = htmlDocument.DocumentNode.Descendants("tr").Where(node => node.GetAttributeValue("class", "").Contains("athleteSearch")).ToList();

            List<Swimmer> selectSwimmers = new List<Swimmer>();
            foreach (var tr in trs)
            {
                var swimmer = GetSwimmerDetailsFromHtmlNode(tr);
                if (swimmer != null)
                {
                    selectSwimmers.Add(swimmer);
                }
            }
            return selectSwimmers;
        }

        public Swimmer GetSwimmerDetailsFromHtmlNode(HtmlNode node)
        {
            var firstNameNode = node.Descendants("td").FirstOrDefault(n => n.HasClass("name"));
            var clubNode = node.Descendants("td").FirstOrDefault(n => n.HasClass("club"));
            var dateNode = node.Descendants("td").FirstOrDefault(n => n.HasClass("date"));
            var genderNode = node.Descendants("img").FirstOrDefault();

            if (firstNameNode == null
                || clubNode == null
                || !int.TryParse(dateNode?.InnerText, out var birthYear)
                || genderNode == null)
            {
                return null;
            }

            var swimmer = new Swimmer
                              {
                                  FirstName = this.GetName(firstNameNode)?[1],
                                  LastName = this.GetName(firstNameNode)?[0],
                                  BirthYear = birthYear,
                                  ID = this.GetID(firstNameNode),
                                  ClubName = this.GetClub(clubNode),
                                  Gender = this.GetGender(genderNode)
                              };
            return swimmer;

        }
        public string[] GetName(HtmlNode node)
        {
            var completeName = node.Descendants("a").FirstOrDefault()?.InnerText;
            var tempName = completeName?.Split(',');

            var names = new List<string>(2);

            if (tempName == null) return null;

            names.AddRange(tempName.Select(name => name.ToLower().Trim(' ')).Select(nameLow => char.ToUpper(nameLow[0]) + nameLow.Substring(1)));

            return names.ToArray();
        }
        public int GetID(HtmlNode node)
        {
            var link = node.Descendants("a").FirstOrDefault()?.ChildAttributes("href").FirstOrDefault()?.Value;
            var splitLink = link?.Split('=');
            
            return Convert.ToInt32(splitLink?[2]);
        }
        public string GetClub(HtmlNode node)
        {
            var completeClub = node.InnerText;
            var tempClub = completeClub.Split('-');
            
            return tempClub[1].Trim();
        }
        public Gender GetGender(HtmlNode node)
        {
            var genderImg = node.OuterHtml;
            var genderNumber = genderImg.Split(new string[] { "gender", "." }, StringSplitOptions.None);
            
            return genderNumber[1] == "1" ? Gender.Male : Gender.Female;
        }
    }
}
