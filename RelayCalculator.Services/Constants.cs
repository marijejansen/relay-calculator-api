using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;

namespace RelayCalculator.Services
{
    public struct Constants
    {
        public static readonly List<int> AgeGroups = new List<int> {80, 100, 120, 160, 200, 240, 280, 320, 360};
        public static readonly List<Gender> GenderGroups = new List<Gender> {Gender.Female, Gender.Male, Gender.Mix};
    }
}
