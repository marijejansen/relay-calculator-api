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

        public struct SwimRankingsPage
        {
            public static readonly Dictionary<string, int> StrokesForRelays = new Dictionary<string, int>()
            {
                {"Freestyle50M", 1},
                {"Freestyle100M", 2},
                {"Freestyle200M", 3},
                {"Backstroke50M", 9},
                {"Backstroke100M", 10},
                {"Breaststroke50M", 12},
                {"Breaststroke100M", 13},
                {"Butterfly50M", 15},
                {"Butterfly100M", 16},
            };

            public static readonly Dictionary<string, int> AllStrokes = new Dictionary<string, int>()
            {
                {"Freestyle50M", 1},
                {"Freestyle100M", 2},
                {"Freestyle200M", 3},
                {"Freestyle400M", 5},
                {"Freestyle800M", 6},
                {"Freestyle1500M", 8},
                {"Backstroke50M", 9},
                {"Backstroke100M", 10},
                {"Backstroke200M", 11},
                {"Breaststroke50M", 12},
                {"Breaststroke100M", 13},
                {"Breaststroke200M", 14},
                {"Butterfly50M", 15},
                {"Butterfly100M", 16},
                {"Butterfly200M", 17},
                {"IndividualMedley200M", 18},
                {"IndividualMedley400M", 19},
                {"IndividualMedley100M", 20},
            };
        }
    }
}
