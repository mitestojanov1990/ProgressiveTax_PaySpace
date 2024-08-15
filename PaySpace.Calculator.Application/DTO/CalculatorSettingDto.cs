using System.Text.Json.Serialization;

namespace PaySpace.Calculator.Application.DTO;

public sealed record CalculatorSettingDto([property: JsonPropertyName("Calculator")] string Calculator,
    [property: JsonPropertyName("RateType")] string RateType,
    [property: JsonPropertyName("Rate")] decimal Rate,
    [property: JsonPropertyName("From")] decimal From,
    [property: JsonPropertyName("To")] decimal? To);