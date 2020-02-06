using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RelayCalculator.Services.Enums;
using RelayCalculator.Services.Interfaces;
using System.Text.Json.Serialization;

namespace RelayCalculator.Services.Models
{
    public class RelayType
    {
        public Distance Distance { get; set; }
        public Stroke Stroke { get; set; }
        public int NumberOfSwimmers { get; set; }

        [JsonIgnore]
        public IBestTeamCalculationService RelayCalculation
        {
            get
            {
                if (Stroke == Stroke.Freestyle)
                {
                    if (Distance == Distance.TwoHundred)
                    {
                        return new Freestyle200Relay();
                    }
                    else if (Distance == Distance.FourHundred)
                    {
                        return new Freestyle400Relay();
                    }
                    else if (Distance == Distance.EightHundred)
                    {
                        return new Freestyle800Relay();
                    }
                }
                else if (Stroke == Stroke.Medley)
                {
                    if (Distance == Distance.TwoHundred)
                    {
                        return new Medley200Relay();
                    }
                    else if (Distance == Distance.FourHundred)
                    {
                        return new Medley400Relay();
                    }
                }

                return null;
            }

            set { }
        }
    }
}
