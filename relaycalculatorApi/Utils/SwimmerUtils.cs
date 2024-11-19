using Microsoft.VisualBasic;
using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RelayCalculator.Api.Utils
{
    public static class SwimmerUtils
    {
        public static string[] GetNameArrayFromString(string name)
        {
            // Bon-rosenbrand van, Lidia
            // Bon-rosenbrand van der, Lidia
            var tempName = name.ToLower()?.Split(',');

            if (tempName == null) return null;

            var firstName = CapitaliseString(tempName[1]);
            var lastNames = tempName[0].Split(' ');
            var lastName = CapitaliseString(lastNames[0]);

            if (lastNames.Length == 2)
            {
                lastName = lastNames[1].Trim(' ') + " " + lastName;
            } else if (lastNames.Length == 3)
            {
                lastName = lastNames[1].Trim(' ') + " " + lastNames[2].Trim(' ') + " " + lastName;
            }

            return new[] {lastName, firstName};
        }

        private static string CapitaliseString(string word)
        {
            word = word.Trim(' ');
            if (!word.Contains("-")) return char.ToUpper(word[0]) + word.Substring(1);
            var split = word.Split('-');
            return CapitaliseString(split[0]) + "-" + CapitaliseString(split[1]);
        }

        public static Gender GetGenderFromString(string name)
        {
            switch(name.ToLower()) {
                case "men":
                case "heren":
                    return Gender.Male;
                case "women":
                case "dames":
                    return Gender.Female;
                case "mixed":
                case "gemengd":
                    return Gender.Mix;
                default: return Gender.Unknown;
            }
        }

        public static SwimEvent GetEventFromString(string eventString)
        {
            var splitted = eventString.Split(' ');
            var distance = int.Parse(splitted[0].Replace("m", ""));
            var stroke = splitted[1];
            switch (stroke.ToLower())
            {
                case "freestyle":
                    return new SwimEvent { Distance = (Distance)distance, Stroke = Stroke.Freestyle };
                case "backstroke":
                    return new SwimEvent { Distance = (Distance)distance, Stroke = Stroke.Backstroke };
                case "breaststroke":
                    return new SwimEvent { Distance = (Distance)distance, Stroke = Stroke.Breaststroke };
                case "butterfly":
                    return new SwimEvent { Distance = (Distance)distance, Stroke = Stroke.Butterfly };
                case "medley":
                    return new SwimEvent { Distance = (Distance)distance, Stroke = Stroke.Medley };
                default:
                    return new SwimEvent { };
            }
        }

        public static Relay? GetRelayEventFromString(string eventString)
        {
            var splitted = eventString.Split(' ');
            var distance = int.Parse(splitted[2].Replace("m", ""));
            var stroke = splitted[3].ToLower();
            switch (stroke.ToLower())
            {
                case "freestyle":
                case "vrije":
                    return distance switch
                    {
                        50 => Relay.Freestyle200,
                        100 => Relay.Freestyle400,
                        200 => Relay.Freestyle800,
                        _ => null  // Return null for unsupported distances
                    };

                case "medley":
                case "wisselslag":
                    return distance switch
                    {
                        50 => Relay.Medley200,
                        100 => Relay.Medley400,
                        _ => null  // Return null for unsupported distances
                    };

                default:
                    return null; // Return null if stroke is unsupported
            }
        }

        public static Stroke GetStrokeFromStringDutch(string eventString)
        {
            var stroke = "";

            try
            {
                stroke = eventString.Split(' ')[1];
            }
            catch
            {
                var x = 1;
            }

            switch (stroke.Trim().ToLower())
            {
                case "vrij":
                case "vrije":
                case "vrije slag":
                    return Stroke.Freestyle;
                case "rug":
                case "rugslag":
                    return Stroke.Backstroke;
                case "school":
                case "schoolslag":
                    return Stroke.Breaststroke;
                case "vlinder":
                case "vlinderslag":
                    return Stroke.Butterfly;
                case "wissel":
                case "wisselslag":
                    return Stroke.Medley;
                default:
                    return Stroke.Unknown;

            }
        }

        public static Distance GetDistanceFromStringDutch(string eventString)
        {
            var distance = "";

            try
            {
                distance = eventString.Split(' ')[0];
            }
            catch
            {
                var x = 1;
            }
            switch (distance)
            {
                case "50":
                    return Distance.Fifty;
                case "100":
                    return Distance.Hundred;
                case "200":
                    return Distance.TwoHundred;
                case "400":
                    return Distance.FourHundred;
                case "800":
                    return Distance.EightHundred;
                case "1500":
                    return Distance.FifteenHundred;
                default:
                    return Distance.TwentyFive;

            }
        }

        public static double ConvertTimeStringToDouble(string time)
        {
            time = Regex.Replace(time, @"[^0-9:.,]", "");
            double timeInSeconds;
            if (time.Contains(":"))
            {
                var splitted = time.Split(':');
                var minutes = Convert.ToDouble(splitted[0]);
                timeInSeconds = minutes * 60;
                var seconds = Convert.ToDouble(splitted[1], CultureInfo.InvariantCulture);
                timeInSeconds += seconds;
            }
            else
            {
                double.TryParse(time, NumberStyles.Any, CultureInfo.InvariantCulture, out timeInSeconds);
            }
            return Math.Round(timeInSeconds, 2);
        }

        public static string ConvertDoubleToTimeString(double seconds)
        {
            var minutes = (seconds - (seconds % 60)) / 60;
            var hundSec = Math.Round(((seconds % 1) * 100), 2);
            var sec = (seconds - (seconds % 1)) - minutes * 60;
            //var sec = seconds - (minutes * 60) - (hundSec / 100);
            var result = "";
            if (minutes > 0)
            {
                result += minutes + ":";
            }

            result += sec.ToString().PadLeft(2, '0') + "." + hundSec.ToString().PadLeft(2, '0');
            return result;
        }

        public static Course GetCourseFromString(string courseString)
        {
            var lowerCourseString = courseString.ToLower();
            if (lowerCourseString.Contains("25") || lowerCourseString.Contains("short") || lowerCourseString.Contains("kort"))
                return Course.Short;
            if (lowerCourseString.Contains("50") || lowerCourseString.Contains("long") || lowerCourseString.Contains("lang"))
                return Course.Long;

            return Course.Short;
        }

        public static double GetTimeFromTimeString(string timeString)
        {
            if (timeString == "") return 0;
            var split = timeString.Split(new string[] { ".", ",", ":" }, StringSplitOptions.RemoveEmptyEntries);
            var hunSec = int.Parse(split[split.Length - 1]);
            if (!int.TryParse(split[split.Length - 2], out _))
            {
                var x = 10;
            }
            var sec = int.Parse(split[split.Length - 2]);

            double time = sec + ((double)hunSec / 100);

            if (split.Length == 3)
            {
                time += (int.Parse(split[0]) * 60);
            }
            return time;
        }

        public static DateTime GetDateFromDateStringShort(string dateString)
        {
            if (dateString == "") return DateTime.Now;
            var split = dateString.Split("/");
            var x = new DateTime(int.Parse(split[2].Split(" ")[0]), int.Parse(split[1]), int.Parse(split[0]), 0, 0, 0, DateTimeKind.Utc);
            //var y = new DateTime(int.Parse(split[2].Split(" ")[0]), int.Parse(split[1]), int.Parse(split[0]));
            return x;
        }

        public static DateTime GetDateFromDateString(string dateString)
        {
            var months = new []
                {"jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec"};
            if (dateString == "") return DateTime.Now;
            var split = dateString.Split(" ");
            var x = new DateTime(int.Parse(split[split.Length-1].Split(" ")[0]), Array.IndexOf(months, split[split.Length - 2].ToLower())+1, int.Parse(split[0]));
            return x;
        }

        public static string GenderToDutchString(Gender gender)
        {
            switch (gender)
            {
                case Gender.Female:
                    return "D";
                case Gender.Male:
                    return "M";
                case Gender.Mix:
                    return "MIX";
                default:
                    return "";
            }
        }
        public static string StrokeToDutchString(Stroke stroke)
        {
            switch (stroke)
            {
                case Stroke.Freestyle:
                    return "vrije slag";
                case Stroke.Backstroke:
                    return "rugslag";
                case Stroke.Breaststroke:
                    return "schoolslag";
                case Stroke.Butterfly:
                    return "vlinderslag";
                case Stroke.Medley:
                    return "wisselslag";
                default:
                    return "";
            }
        }

        public static string CourseToDutchShorthand(Course course)
        {
            switch (course)
            {
                case Course.Short:
                    return "KB";
                case Course.Long:
                    return "LB";
                default: return "";
            }
        }

        public static int GetRelayAgeGroupForTotalAge(int totalYears)
        {
            if (totalYears >= 100 && totalYears < 120)
            {
                return 100;
            }

            return ((int)totalYears / 40) * 40;
        }
    }
}
