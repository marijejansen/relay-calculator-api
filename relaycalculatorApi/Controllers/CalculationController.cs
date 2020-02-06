using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RelayCalculator.Api.Mapper;
using RelayCalculator.Api.Models;
using RelayCalculator.Services;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;
using RelayCalculator.Services.Enums;
using Distance = RelayCalculator.Services.Enums.Distance;
using Stroke = RelayCalculator.Services.Enums.Stroke;
using SwimmerModel = RelayCalculator.Api.Models.SwimmerModel;


namespace RelayCalculator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService _calculationService;

        public CalculationController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        [HttpPost]
        [Route("getBestTeams")]
        public List<RelayTeam> GetBestRelayTeams(CalculationRequest request)
        {
            // TODO: dependency injection
            var mapper = new SwimmerMapper();
            var swimmers = request.Swimmers.Select(s => mapper.Map(s)).ToList();
            return _calculationService.BestRelayTeams(swimmers, request.RelayType);
        }

        [HttpGet]
        [Route("getCalculationRequest")]
        public CalculationRequest GetCalculationRequest()
        {
            return new CalculationRequest
            {
                RelayType = new RelayType
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Freestyle,
                    NumberOfSwimmers = 4
                },
                Swimmers = new List<SwimmerModel>()
                {
                    new SwimmerModel()
                    {
                        FirstName = "Marije",
                        LastName = "Jansen",
                        BirthYear = 1985,
                        Gender = Gender.Female,
                        ShortCourseTimes = new CourseTimes()
                        {
                            Freestyle50M = 28.61,
                            Freestyle100M = 62.88
                        }
                    },
                    new SwimmerModel()
                    {
                        FirstName = "Martijn",
                        LastName = "Giezen",
                        BirthYear = 1985,
                        Gender = Gender.Male,
                        ShortCourseTimes = new CourseTimes()
                        {
                            Freestyle50M = 26.61,
                            Freestyle100M = 60.21
                        }
                    },
                    new SwimmerModel()
                    {
                        FirstName = "Elin",
                        LastName = "Giezen",
                        BirthYear = 1972,
                        Gender = Gender.Female,
                        ShortCourseTimes = new CourseTimes()
                        {
                            Freestyle50M = 36.61,
                            Freestyle100M = 92.21
                        }
                    },
                    new SwimmerModel()
                    {
                        FirstName = "Moskou",
                        LastName = "Jansen",
                        BirthYear = 2000,
                        Gender = Gender.Male,
                        ShortCourseTimes = new CourseTimes()
                        {
                            Freestyle50M = 32.23,
                            Freestyle100M = 71.91
                        }
                    },
                    new SwimmerModel()
                    {
                        FirstName = "Brussel",
                        LastName = "Jansen",
                        BirthYear = 1998,
                        Gender = Gender.Male,
                        ShortCourseTimes = new CourseTimes()
                        {
                            Freestyle50M = 27.23,
                            Freestyle100M = 61.91
                        }
                    },
                }
            };
        }

        [HttpPost]
        [Route("testpost")]
        public string Test(string test)
        {
            test += _calculationService.GetString();
            return "TEST " + test + " TEST";
        }
    }
}
