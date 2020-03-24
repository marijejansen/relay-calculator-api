using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Interfaces;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IPermutationService _permutationService;
        private readonly IGroupService _groupService;
        private readonly IBestTeamCalculationService _bestTeamCalculationService;

        public CalculationService(IPermutationService permutationService, IGroupService groupService, IBestTeamCalculationService bestTeamCalculationService)
        {
            _permutationService = permutationService;
            _groupService = groupService;
            _bestTeamCalculationService = bestTeamCalculationService;
        }

        public List<RelayTeam> BestRelayTeams(List<Swimmer> swimmers, RelayType relayType, Course courseType)
        {
            var permutations = _permutationService.GetPermutations(swimmers.Count());

            var bestTeams = new List<RelayTeam>();

            foreach (var gender in Constants.GenderGroups)
            {
                var teamsGender = permutations.Where(p => _groupService.GetGenderGroup(p, swimmers) == gender).ToList();

                foreach (var age in Constants.AgeGroups)
                {
                    var teamsAge = teamsGender.Where(p => _groupService.GetAgeGroup(p, swimmers) == age).ToList();
                    if (!(teamsAge.Count > 0)) continue;
                    
                    //TODO: get these into a model?
                    var bestTeam = GetBestTeam(swimmers, teamsAge, relayType, courseType);

                    bestTeam.Gender = gender;
                    bestTeam.Age = age;

                    bestTeams.Add(bestTeam);
                }
            }

            return bestTeams;
        }

        public RelayTeam GetBestTeam(List<Swimmer> swimmers, List<int[]> possibleTeams, RelayType relayType, Course course)
        {
            var bestTime = 0.0;
            var bestTeam = new int[4];

            foreach (var team in possibleTeams)
            {
                var time = relayType.RelayCalculation?.GetTime(team, swimmers, course);

                if (time == null || !(time > 0) || (!(time < bestTime) && bestTime > 0)) continue;

                bestTime = time.GetValueOrDefault();
                bestTeam = team;
            }

            return new RelayTeam
            {
                Swimmers = relayType.RelayCalculation?.GetRelaySwimmersByPermutation(bestTeam, swimmers, course),
                Time = bestTime
            };
        }
    }
}
