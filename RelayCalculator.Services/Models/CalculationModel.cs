using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Interfaces;

namespace RelayCalculator.Services.Models
{
    public class CalculationModel
    {
        public List<Swimmer> Swimmers { get; set; }

        public Course Course { get; set; }

        public Relay Relay { get; set; }

        public bool MastersAgeGroups { get; set; }

        [JsonIgnore]
        public IBestTeamCalculationService RelayCalculation
        {
            get
            {
                switch (Relay)
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
                    default:
                        return null;
                }
            }
        }
    }
}
