using RelayCalculator.Api.Models;
using RelayCalculator.Api.Services.Models;

namespace RelayCalculator.Api.Mapper
{
    public interface ISwimmerMapper
    {
        Swimmer Map(SwimmerModel swimmer);
        SwimmerModel ReverseMap(Swimmer swimmer);

    }
}
