using Azure;
using Azure.Data.Tables;
using RelayCalculator.Api.Services.Models;
using System;

namespace RelayCalculator.Api.Services.Entities
{
    public class RecordEntity : ITableEntity
    {
        public RecordEntity() {
            this.PartitionKey = string.Empty;
            this.RowKey = string.Empty;
        }
        public RecordEntity(ClubRecord clubRecord)
        {
            var prefix = clubRecord.IsRelay ? "R_" : "";
            PartitionKey = $"{prefix}{clubRecord.Gender}_{clubRecord.AgeGroup}_{clubRecord.Course}";
            RowKey = $"{clubRecord.Stroke}_{(int)clubRecord.Distance}";
            Name = clubRecord?.Name;
            if (clubRecord?.Time > 0) Time = clubRecord.Time;
            if (clubRecord?.Date > DateTime.MinValue) RecordDate = clubRecord.Date;
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string? Name { get; set; }
        public double Time { get; set; }
        public DateTimeOffset RecordDate { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
