namespace RelayCalculator.Api.Models
{
    using System.Collections.Generic;
    using RelayCalculator.Services.Enums;

    public class CalculationRequest
    {
        public IEnumerable<SwimmerModel> Swimmers
        {
            get; 
            set;
        }

        public Course Course
        {
            get; 
            set;
        }

        public Relay Relay
        {
            get;
            set;
        }

        public int? CalculateForYear
        {
            get;
            set;
        }
    }
}
