using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RelayCalculator.Api.Models;
using RelayCalculator.Services.Models;

namespace RelayCalculator.Api.Mapper
{
    public interface ISwimmerMapper
    {
        Swimmer Map(SwimmerModel swimmer);
        SwimmerModel ReverseMap(Swimmer swimmer);

    }
}
