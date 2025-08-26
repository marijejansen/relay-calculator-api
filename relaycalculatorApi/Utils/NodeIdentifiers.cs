using RelayCalculator.Api.Services.Enums;
using System;
using System.Xml;

namespace RelayCalculator.Api.Utils
{
    public static class NodeIdentifiers
    {
        public static double GetTimeForRecord(XmlNode node)
        {
            var time = node.GetAttributeValue("swimtime");
            if (time == null)
            {
                return 0;
            }

            var splitTime = time.Split(":");

            var hours = int.Parse(splitTime[0]);
            var minutes = int.Parse(splitTime[1]);
            var seconds = double.Parse(splitTime[2].Split(".")[0]);
            var milSeconds = double.Parse(
                splitTime[2]
                    .Split(".")[1]);

            seconds += (((hours * 60) + minutes) * 60) + (milSeconds / 100);

            return seconds;
        }

        public static string DoubleToStringTime(double time)
        {
            var min = Math.Floor(time / 60);
            var minString = min.ToString().PadLeft(2, '0');
            var sec = Math.Floor(time - min * 60);
            var secString = sec.ToString().PadLeft(2, '0');
            var milSec = (time - min * 60 - sec) * 100;
            var milSecString = Math.Round(milSec, 2).ToString().PadLeft(2, '0');

            var timeString = $"{minString}:{secString}.{milSecString}";
            return timeString;
        }

        public static RelayType? GetRelayTypeForRecord(XmlNode node)
        {
            var swimStyleNode = GetSwimStyleNode(node);
            var distance = swimStyleNode?.GetAttributeValue("distance");

            switch (swimStyleNode?.GetAttributeValue("stroke"))
            {
                case "FREE" when distance == "50":
                    return RelayType.Freestyle200;
                case "FREE" when distance == "100":
                    return RelayType.Freestyle400;
                case "FREE" when distance == "200":
                    return RelayType.Freestyle800;
                case "MEDLEY" when distance == "50":
                    return RelayType.Medley200;
                case "MEDLEY" when distance == "100":
                    return RelayType.Medley400;
                default:
                    return null;
            }
        }

        public static Course? GetCourseForRecordGroup(XmlNode node)
        {
            switch (node.GetAttributeValue("course"))
            {
                case "SCM":
                    return Course.Short;
                case "LCM":
                    return Course.Long;
                default:
                    return null;
            }
        }

        public static int? GetAgeForRecordGroup(XmlNode node)
        {
            var ageGroupNode = GetAgeGroupNode(node);
            var age = ageGroupNode?.GetAttributeValue("agemin");

            if (!int.TryParse(age, out var groupAge))
            {
                return null;
            }

            return groupAge;
        }

        public static Gender? GetGenderForRecordsGroup(XmlNode node)
        {
            switch (node.GetAttributeValue("gender"))
            {
                case "F":
                    return Gender.Female;
                case "M":
                    return Gender.Male;
                case "X":
                    return Gender.Mix;
                default:
                    return null;
            }
        }

        public static RecordType? GetRecordTypeForRecordsGroup(XmlNode node)
        {
            switch (node.GetAttributeValue("type"))
            {
                case "NMR":
                    return RecordType.NMR;
                case "EMR":
                    return RecordType.EMR;
                case "WMR":
                    return RecordType.WMR;
                default:
                    return null;
            }
        }

        public static Stroke? GetStroke(XmlNode node)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "SWIMSTYLE")
                {
                    switch (childNode.GetAttributeValue("stroke"))
                    {
                        case "FREE":
                            return Stroke.Freestyle;
                        case "BACK":
                            return Stroke.Backstroke;
                        case "BREAST":
                            return Stroke.Breaststroke;
                        case "FLY":
                            return Stroke.Butterfly;
                        case "MEDLEY":
                            return Stroke.Medley;
                        default:
                            return null;
                    }
                }
            }

            return null;
        }

        public static Distance? GetDistance(XmlNode node)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "SWIMSTYLE")
                {
                    switch (childNode.GetAttributeValue("distance"))
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
                            return null;
                    }
                }
            }

            return null;
        }

        public static bool GetNodeIsRelayRecord(XmlNode node)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "AGEGROUP" && childNode.GetAttributeValue("calculate") == "TOTAL")
                {
                    return true;
                }
            }

            return false;
        }

        public static bool GetNodeIsIndividualRecord(XmlNode node)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "AGEGROUP" && childNode.GetAttributeValue("calculate") != "TOTAL")
                {
                    return true;
                }
            }

            return false;
        }

        public static XmlNode? GetAgeGroupNode(XmlNode node)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "AGEGROUP")
                {
                    return childNode;
                }
            }

            return null;
        }

        public static XmlNode? GetSwimStyleNode(XmlNode node)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "SWIMSTYLE")
                {
                    return childNode;
                }
            }

            return null;
        }

        private static string? GetAttributeValue(this XmlNode node, string attributeName)
        {
            return node.Attributes?[attributeName]?.Value;
        }
    }
}