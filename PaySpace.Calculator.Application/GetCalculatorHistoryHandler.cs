using Mediator;
using Microsoft.Extensions.Logging;
using PaySpace.Calculator.Application.Abstractions;
using System.Text.Json.Serialization;

namespace PaySpace.Calculator.Application;

public sealed record GetCalculatorHistoryRequest : IQuery<IEnumerable<CalculatorHistoryDto>>;
public sealed record CalculatorHistoryDto
{
    [property: JsonPropertyName("PostalCode")]
    public string PostalCode { get; set; }
    [property: JsonPropertyName("Income")]
    public decimal Income { get; set; }
    [property: JsonPropertyName("Tax")]
    public decimal Tax { get; set; }
    [property: JsonPropertyName("Calculator")]
    public string Calculator { get; set; }
    [property: JsonPropertyName("Timestamp")]
    public DateTime Timestamp { get; set; }
}

public sealed class GetCalculatorHistoryHandler : IQueryHandler<GetCalculatorHistoryRequest, IEnumerable<CalculatorHistoryDto>>
{
    private readonly ILogger<GetCalculatorHistoryHandler> _logger;
    private readonly IHistoryService _historyService;
    public GetCalculatorHistoryHandler(ILogger<GetCalculatorHistoryHandler> logger, IHistoryService historyService) =>
        (_logger, _historyService) = (logger, historyService);
    public async ValueTask<IEnumerable<CalculatorHistoryDto>> Handle(GetCalculatorHistoryRequest request, CancellationToken cancellationToken)
    {
        return await _historyService.GetHistoryAsync(cancellationToken);
    }
}
