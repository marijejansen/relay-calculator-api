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

        public List<RelayTeam> BestRelayTeams(CalculationModel calculationModel)
        {
            var swimmers = calculationModel.Swimmers;

            var permutations = _permutationService.GetPermutations(swimmers.Count());

            var bestTeams = new List<RelayTeam>();

            foreach (var gender in Constants.GenderGroups)
            {
                var teamsGender = permutations.Where(p => _groupService.GetGenderGroup(p, swimmers) == gender).ToList();

                foreach (var age in Constants.AgeGroups)
                {
                    var teamsAge = teamsGender.Where(p => _groupService.GetAgeGroup(p, swimmers) == age).ToList();
                    if (!(teamsAge.Count > 0)) continue;
                    
                    var bestTeam = GetBestTeam(teamsAge, calculationModel);
                    if (bestTeam == null) continue;

                    bestTeam.Gender = gender;
                    bestTeam.AgeGroup = age;
                    bestTeams.Add(bestTeam);
                }
            }

            return bestTeams;
        }

        public RelayTeam GetBestTeam(List<int[]> possibleTeams, CalculationModel calculationModel)
        {
            var bestTime = 0.0;
            var bestTeam = new int[4];

            foreach (var team in possibleTeams)
            {
                var time = calculationModel.RelayCalculation.GetTime(team, calculationModel.Swimmers, calculationModel.Course);

                if (!(time > 0) || (!(time < bestTime) && bestTime > 0)) continue;

                bestTime = time;
                bestTeam = team;
            }

            return bestTime > 0 ? new RelayTeam
            {
                Swimmers = calculationModel.RelayCalculation.GetRelaySwimmersByPermutation(bestTeam, calculationModel.Swimmers, calculationModel.Course),
                Time = bestTime,
                Age = _groupService.GetAge(bestTeam, calculationModel.Swimmers),
            } : null;
        }
    }
}
