using RelayCalculator.Api.Services.Enums;
using System;

namespace RelayCalculator.Api.Services.Models
{
    public class ClubRecord
    {
        public string Name { 
            get; 
            set; 
        }

        public Distance Distance
        {
            get;
            set;
        }

        public Stroke Stroke
        {
            get;
            set;
        }

        public double Time
        {
            get;
            set;
        }

        public int AgeGroup
        {
            get;
            set;
        }

        public Gender Gender
        {
            get;
            set;
        }

        public Course Course
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public bool IsRelay
        {
            get; 
            set;
        }
    }
}
