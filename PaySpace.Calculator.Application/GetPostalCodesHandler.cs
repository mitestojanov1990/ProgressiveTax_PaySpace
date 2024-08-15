using Mediator;
using Microsoft.Extensions.Logging;
using PaySpace.Calculator.Application.Abstractions;
using System.Text.Json.Serialization;

namespace PaySpace.Calculator.Application;

public sealed record GetPostalCodesRequest : IQuery<IEnumerable<PostalCodeDto>>;
public sealed record PostalCodeDto([property: JsonPropertyName("Code")] string Code, [property: JsonPropertyName("Calculator")] string Calculator);
public sealed class GetPostalCodesHandler : IQueryHandler<GetPostalCodesRequest, IEnumerable<PostalCodeDto>>
{
    private readonly ILogger<GetPostalCodesHandler> _logger;
    private readonly IPostalCodeService _postalCodeService;
    public GetPostalCodesHandler(ILogger<GetPostalCodesHandler> logger, IPostalCodeService postalCodeService) =>
        (_logger, _postalCodeService) = (logger, postalCodeService);
    public async ValueTask<IEnumerable<PostalCodeDto>> Handle(GetPostalCodesRequest request, CancellationToken cancellationToken)
    {
        return await _postalCodeService.GetPostalCodesAsync(cancellationToken);
    }
}
