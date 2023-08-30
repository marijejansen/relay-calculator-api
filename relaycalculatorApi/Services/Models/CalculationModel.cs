using System.Collections.Generic;
using System.Text.Json.Serialization;
using RelayCalculator.Api.Services.Enums;
using RelayCalculator.Api.Services.Interfaces;

namespace RelayCalculator.Api.Services.Models
{
    public class CalculationModel
    {
        public List<Swimmer> Swimmers
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

        public bool MastersAgeGroups
        {
            get;
            set;
        }

        [JsonIgnore]
        public IBestTeamCalculationService RelayCalculation
        {
            get
            {
                switch (this.Relay)
                {
                    case Relay.Freestyle200:
                        return new Freestyle200Relay();

                    case Relay.Freestyle400:
                        return new Freestyle400Relay();

                    case Relay.Freestyle800:
                        return new Freestyle800Relay();

                    case Relay.Medley200:
                        return new Medley200Relay();

                    case Relay.Medley400:
                        return new Medley400Relay();

                    case Relay.Backstroke200:
                        return new Backstroke200Relay();

                    case Relay.Breaststroke200:
                        return new Breaststroke200Relay();

                    default:
                        return null;
                }
            }
        }
    }
}