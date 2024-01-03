using RelayCalculator.Api.Services.Enums;

namespace RelayCalculator.Api.Models
{
    public class RacePerformance
    {
        public Distance Distance { get; set; }
        public Stroke Stroke { get; set; }
        public double Time { get; set; }
        public double Percentage { get; set; }
        //public double[]? Splits { get; set; }
    } 
}
