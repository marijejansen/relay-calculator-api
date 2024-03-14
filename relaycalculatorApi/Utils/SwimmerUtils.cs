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
    }
}
