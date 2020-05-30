using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelayCalculator.Services.Models
{
    using RelayCalculator.Services.Enums;

    public class RelayRecord
    {
        public double Time
        {
            get;
            set;
        }

        public int Age
        {
            get;
            set;
        }

        public Relay Relay
        {
            get;
            set;
        }

        public string Area
        {
            get;
            set;
        }
    }
}
