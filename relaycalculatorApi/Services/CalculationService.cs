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
        private readonly IPermutationService permutationService;

        private readonly IGroupService groupService;

        private readonly IBestTeamCalculationService bestTeamCalculationService;

        public CalculationService(IPermutationService permutationService, IGroupService groupService, IBestTeamCalculationService bestTeamCalculationService)
        {
            this.permutationService = permutationService;
            this.groupService = groupService;
            this.bestTeamCalculationService = bestTeamCalculationService;
        }

        public List<RelayTeam> BestRelayTeams(CalculationModel calculationModel)
        {
            var swimmers = calculationModel.Swimmers;

            var permutations = this.permutationService.GetPermutations(swimmers.Count());

            var bestTeams = new List<RelayTeam>();

            foreach (var gender in Constants.GenderGroups)
            {
                var teamsGender = permutations.Where(p => this.groupService.GetGenderGroup(p, swimmers) == gender).ToList();

                var masters = calculationModel.MastersAgeGroups;
                var groups = !masters ? new List<int>() { 0 } : Constants.AgeGroups;

                foreach (var age in groups)
                {
                    var teamsAge = masters
                        ? teamsGender.Where(p => this.groupService.GetAgeGroupForOrder(p, swimmers, calculationModel.CalculateForYear) == age).ToList()
                        : teamsGender;
                    if (!(teamsAge.Count > 0))
                    {
                        continue;
                    }

                    var bestTeam = this.GetBestTeam(teamsAge, calculationModel);
                    if (bestTeam == null)
                    {
                        continue;
                    }

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
            var bestTeam = new List<Swimmer>();

            foreach (var team in possibleTeams)
            {
                var teamSwimmers = team.Select(i => calculationModel.Swimmers[i]).ToList();
                var time = calculationModel.RelayCalculation.GetTime(team, calculationModel.Swimmers, calculationModel.Course);

                if (!(time > 0)
                    || (!(time < bestTime) && bestTime > 0))
                {
                    continue;
                }

                bestTime = time;
                bestTeam = teamSwimmers;
            }

            return bestTime > 0 ? new RelayTeam
            {
                Swimmers = calculationModel.RelayCalculation.GetRelaySwimmers(bestTeam, calculationModel.Course),
                Time = bestTime,
                Age = this.groupService.GetAge(bestTeam, calculationModel.CalculateForYear),
            } 
            : null;
        }
    }
}
