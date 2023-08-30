// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalculationController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CalculationController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using RelayCalculator.Api.Mapper;
    using RelayCalculator.Api.Models;
    using SwimmerModel = RelayCalculator.Api.Models.SwimmerModel;

    /// <summary>
    /// Calculation controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService calculationService;

        private readonly ISwimmerMapper swimmerMapper;

        public CalculationController(ICalculationService calculationService, ISwimmerMapper swimmerMapper)
        {
            this.calculationService = calculationService;
            this.swimmerMapper = swimmerMapper;
        }

        /// <summary>Calculates the fastest team of each gender group no age groups
        /// for the given swimmers and relay type.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns a list of all best teams.</returns>
        [HttpPost]
        [Route("getBestTeams")]
        public List<RelayTeam> GetBestRelayTeams(CalculationRequest request)
        {
            return this.calculationService.BestRelayTeams(new CalculationModel()
            {
                Swimmers = request.Swimmers.Select(s => this.swimmerMapper.Map(s)).ToList(),
                Relay = request.Relay,
                Course = request.Course,
                CalculateForYear = request.CalculateForYear
            });
        }

        /// <summary>
        /// Calculates the fastest team of each possible age group and gender
        /// for the given swimmers and relay type.
        /// </summary>
        /// <returns>
        /// Returns a list of all best teams.
        /// </returns>
        [HttpPost]
        [Route("getBestMastersTeams")]
        public List<RelayTeam> GetBestMastersRelayTeams(CalculationRequest request)
        {
            return this.calculationService.BestRelayTeams(new CalculationModel()
            {
                Swimmers = request.Swimmers.Select(s => this.swimmerMapper.Map(s)).ToList(),
                Relay = request.Relay,
                Course = request.Course,
                CalculateForYear = request.CalculateForYear,
                MastersAgeGroups = true,
            });
        }

        /// <summary>
        /// Returns a sample calculationrequest for testing
        /// </summary>
        /// <returns>
        /// Returns a calculationrequest
        /// </returns>
        [HttpGet]
        [Route("getCalculationRequest")]
        public CalculationRequest GetCalculationRequest()
        {
            return new CalculationRequest
            {
                Relay = Relay.Freestyle200,
                Course = Course.Short,
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
                            Freestyle100M = 62.88,
                            Backstroke50M = 32.99,
                            Breaststroke50M = 43.23,
                            Butterfly50M = 33.01
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
                            Freestyle100M = 60.21,
                            Backstroke50M = 34.19,
                            Breaststroke50M = 33.23,
                            Butterfly50M = 35.01
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
                            Freestyle100M = 92.21,
                            Backstroke50M = 44.19,
                            Breaststroke50M = 43.23,
                            Butterfly50M = 32.91
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
                            Freestyle100M = 71.91,
                            Backstroke50M = 39.27,
                            Breaststroke50M = 40.03,
                            Butterfly50M = 39.43
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
                            Freestyle100M = 61.91,
                            Backstroke50M = 34.67,
                            Breaststroke50M = 38.23,
                            Butterfly50M = 32.61
                        }
                    },
                    new SwimmerModel()
                    {
                        FirstName = "Iemand",
                        LastName = "Anders",
                        BirthYear = 1982,
                        Gender = Gender.Male,
                        ShortCourseTimes = new CourseTimes()
                        {
                            Freestyle50M = 29.46,
                            Freestyle100M = 64.45,
                            Backstroke50M = 32.33,
                            Breaststroke50M = 39.99,
                            Butterfly50M = 30.21
                        }
                    },
                    new SwimmerModel()
                    {
                        FirstName = "Geen",
                        LastName = "Idee",
                        BirthYear = 1952,
                        Gender = Gender.Female,
                        ShortCourseTimes = new CourseTimes()
                        {
                            Freestyle50M = 34.91,
                            Freestyle100M = 81.45,
                            Backstroke50M = 35.32,
                            Breaststroke50M = 42.55,
                            Butterfly50M = 40.22
                        }
                    },
                    new SwimmerModel()
                    {
                        FirstName = "ABC",
                        LastName = "DEF",
                        BirthYear = 1964,
                        Gender = Gender.Female,
                        ShortCourseTimes = new CourseTimes()
                        {
                            Freestyle50M = 32.01,
                            Freestyle100M = 73.81,
                            Backstroke50M = 37.47,
                            Breaststroke50M = 42.55,
                            Butterfly50M = 33.01
                        }
                    },
                }
            };
        }
    }
}
