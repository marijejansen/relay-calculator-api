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
    public class GroupService : IGroupService
    {
        public int GetAgeGroup(int[] order, List<Swimmer> swimmers)
        {
            var totalYears = 0;

            swimmers.ForEach(s => totalYears += DateTime.Now.Year - s.BirthYear);

            //foreach (var index in order)
            //{
            //    totalYears += (DateTime.Now.Year - swimmers[index].BirthYear);
            //}

            if (totalYears < 80)
            {
                return 0;
            }
            else if (totalYears > 80 && totalYears < 100)
            {
                return 80;
            }
            else if (totalYears < 120)
            {
                return 100;
            }
            else if (totalYears < 160)
            {
                return 120;
            }
            else if (totalYears < 200)
            {
                return 160;
            }
            else if (totalYears < 240)
            {
                return 200;
            }
            else if (totalYears < 280)
            {
                return 240;
            }
            else if (totalYears < 320)
            {
                return 280;
            }
            else if (totalYears < 360)
            {
                return 320;
            }
            else if (totalYears < 400)
            {
                return 360;
            }
            else
            {
                return 0;
            }
        }
        public Gender GetGenderGroup(int[] order, List<Swimmer> swimmers)
        {
            var numOfM = 0;
            var numOfW = 0;
            foreach (var index in order)
            {
                if (swimmers[index].Gender == Gender.Female)
                {
                    numOfW++;
                }
                else if (swimmers[index].Gender == Gender.Male)
                {
                    numOfM++;
                }
            }

            if (numOfW == 4)
            {
                return Gender.Female;
            }
            else if (numOfM == 4)
            {
                return Gender.Male;
            }
            else if (numOfM == 2 && numOfW == 2)
            {
                return Gender.Mix;
            }
            else
            {
                return Gender.Unknown;
            }
        }

        public List<Swimmer> GetSwimmersByPermutation(int[] permutation, List<Swimmer> swimmers)
        {
            var swimmersList = new List<Swimmer>();
            foreach (var number in permutation)
            {
                swimmersList.Add(swimmers[number]);
            }

            return swimmersList;
        }

        public List<RelaySwimmer> GetRelaySwimmersByPermutation(int[] permutation, List<Swimmer> swimmers)
        {
            return permutation.Select(n => swimmers[n]).Select(s => new RelaySwimmer
            {
                FirstName = s.FirstName,
                Age = DateTime.Today.Year - s.BirthYear
            }).ToList();
        }

    }
}
