using Azure;
using Azure.Data.Tables;
using RelayCalculator.Api.Services.Models;
using System;

namespace RelayCalculator.Api.Services.Entities
{
    public class RecordEntity : ITableEntity
    {
        public RecordEntity() { }
        public RecordEntity(ClubRecord clubRecord)
        {
            PartitionKey = $"{clubRecord.Gender}_{clubRecord.AgeGroup}_{clubRecord.Course}";
            RowKey = $"{clubRecord.Stroke}_{(int)clubRecord.Distance}";
            Name = clubRecord.Name;
            if (clubRecord.Time > 0) Time = clubRecord.Time;
            if (clubRecord.Date > DateTime.MinValue) RecordDate = clubRecord.Date;

            //var distance = clubRecord.Distance;
            //var time = clubRecord.Time;

            //switch (clubRecord.Stroke)
            //{
            //    case Stroke.Freestyle:
            //        switch(distance)
            //        {
            //            case Distance.Fifty:
            //                Freestyle50M = time;
            //                break;
            //            case Distance.Hundred:
            //                Freestyle100M = time;
            //                break;
            //            case Distance.TwoHundred:
            //                Freestyle200M = time;
            //                break;
            //            case Distance.FourHundred:
            //                Freestyle400M = time;
            //                break;
            //            case Distance.EightHundred:
            //                Freestyle800M = time;
            //                break;
            //            case Distance.FifteenHundred:
            //                Freestyle1500M = time;
            //                break;
            //            default:
            //                return;
            //        }
            //        break;
            //    case Stroke.Backstroke: 
            //        switch(distance)
            //        {
            //            case Distance.Fifty: 
            //                Backstroke50M = time;
            //                break;
            //            case Distance.Hundred:
            //                Backstroke100M = time;
            //                break;
            //            case Distance.TwoHundred:
            //                Backstroke200M = time;
            //                break;
            //            default: return;
            //        }
            //        break;
            //    case Stroke.Butterfly: 
            //        switch(distance) { 
            //                case Distance.Fifty:
            //                Butterfly50M = time;   
            //                break;
            //            case Distance.Hundred:
            //                Butterfly100M = time;
            //                break;
            //            case Distance.TwoHundred:
            //                Butterfly200M = time;
            //                break;
            //            default: return;
            //        }
            //        break;
            //    case Stroke.Breaststroke: 
            //        switch(distance) {
            //            case Distance.Fifty:
            //                Breaststroke50M = time;
            //                break;
            //            case Distance.Hundred:
            //                Breaststroke100M = time;
            //                break;
            //            case Distance.TwoHundred:
            //                Breaststroke200M = time;
            //                break;
            //            default: return;
            //        }
            //        break;
            //    case Stroke.Medley:
            //        switch(distance)
            //        {
            //            case Distance.Hundred:
            //                IndividualMedley100M = time;
            //                break;
            //            case Distance.TwoHundred:
            //                IndividualMedley200M = time;
            //                break;
            //            case Distance.FourHundred:
            //                IndividualMedley100M = time;
            //                break;
            //            default: return;
            //        }
            //        break;
            //    default: return;
            //}
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Name { get; set; }
        public double Time { get; set; }
        public DateTimeOffset RecordDate { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
