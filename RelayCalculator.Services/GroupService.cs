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
        public int GetAge(List<Swimmer> swimmers, int? forYear = null)
        {
            var calcYear = forYear ?? DateTime.Now.Year;
            return swimmers.Sum(swimmer => calcYear - swimmer.BirthYear);
        }

        public int GetAgeGroupForOrder(int[] order, List<Swimmer> swimmers, int? forYear = null)
        {
            var teamSwimmers = order.Select(i => swimmers[i]).ToList();

            var calcYear = forYear ?? DateTime.Now.Year;
            if (teamSwimmers.Any(sw => calcYear - sw.BirthYear < 20))
            {
                return 0;
            }

            var totalYears = this.GetAge(teamSwimmers, forYear);

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
    }
}
