using System.Collections.Generic;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Enums;

namespace RelayCalculator.Api.Services
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

        public struct Individual
        {
            public static readonly List<int> AgeGroups = new List<int> { 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80 };
            public static readonly List<Gender> GenderGroups = new List<Gender> { Gender.Female, Gender.Male, Gender.Mix };
            public static readonly List<SwimEvent> AllSwimEventsLongCourse = new List<SwimEvent>() {
                // freestyle
                new SwimEvent()
                {
                    Distance = Distance.Fifty,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.Hundred,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.FourHundred,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.EightHundred,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.FifteenHundred,
                    Stroke = Stroke.Freestyle
                },
                // backstroke
                new SwimEvent()
                {
                    Distance = Distance.Fifty,
                    Stroke = Stroke.Backstroke
                },
                new SwimEvent()
                {
                    Distance = Distance.Hundred,
                    Stroke = Stroke.Backstroke
                },
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Backstroke
                },
                // breaststroke
                new SwimEvent()
                {
                    Distance = Distance.Fifty,
                    Stroke = Stroke.Breaststroke
                },
                new SwimEvent()
                {
                    Distance = Distance.Hundred,
                    Stroke = Stroke.Breaststroke
                },
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Breaststroke
                },
                // butterfly
                new SwimEvent()
                {
                    Distance = Distance.Fifty,
                    Stroke = Stroke.Butterfly
                },
                new SwimEvent()
                {
                    Distance = Distance.Hundred,
                    Stroke = Stroke.Butterfly
                },
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Butterfly
                },
                // medley
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Medley
                },
                new SwimEvent()
                {
                    Distance = Distance.FourHundred,
                    Stroke = Stroke.Medley
                },
            };

            public static readonly List<SwimEvent> AllSwimEventsShortCourse = new List<SwimEvent>()
            {
                // freestyle
                new SwimEvent()
                {
                    Distance = Distance.Fifty,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.Hundred,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.FourHundred,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.EightHundred,
                    Stroke = Stroke.Freestyle
                },
                new SwimEvent()
                {
                    Distance = Distance.FifteenHundred,
                    Stroke = Stroke.Freestyle
                },
                // backstroke
                new SwimEvent()
                {
                    Distance = Distance.Fifty,
                    Stroke = Stroke.Backstroke
                },
                new SwimEvent()
                {
                    Distance = Distance.Hundred,
                    Stroke = Stroke.Backstroke
                },
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Backstroke
                },
                // breaststroke
                new SwimEvent()
                {
                    Distance = Distance.Fifty,
                    Stroke = Stroke.Breaststroke
                },
                new SwimEvent()
                {
                    Distance = Distance.Hundred,
                    Stroke = Stroke.Breaststroke
                },
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Breaststroke
                },
                // butterfly
                new SwimEvent()
                {
                    Distance = Distance.Fifty,
                    Stroke = Stroke.Butterfly
                },
                new SwimEvent()
                {
                    Distance = Distance.Hundred,
                    Stroke = Stroke.Butterfly
                },
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Butterfly
                },
                // medley
                new SwimEvent()
                {
                    Distance = Distance.Hundred,
                    Stroke = Stroke.Medley
                },
                new SwimEvent()
                {
                    Distance = Distance.TwoHundred,
                    Stroke = Stroke.Medley
                },
                new SwimEvent()
                {
                    Distance = Distance.FourHundred,
                    Stroke = Stroke.Medley
                },
            };

        }
    }
}
