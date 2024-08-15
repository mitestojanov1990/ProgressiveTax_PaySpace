using Mapster;
using PaySpace.Calculator.Application;
using PaySpace.Calculator.Domain;

namespace PaySpace.Calculator.Infrastructure.Mapping;

public static class MapsterSettings
{
    public static void Configure()
    {
        TypeAdapterConfig.GlobalSettings.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);

        TypeAdapterConfig<CalculatorHistory, CalculatorHistoryDto>.NewConfig();
    }
}
