using Mediator;
using PaySpace.Calculator.Application.Abstractions;
using System.Net;
using FluentValidation;
using System.Text.Json.Serialization;

namespace PaySpace.Calculator.Application;

public sealed class CalculateRequestValidator : AbstractValidator<CalculateRequest>
{
    public CalculateRequestValidator(IPostalCodeService postalCodeService)
    {
        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .MaximumLength(4)
            .MustAsync(async (request, postalCode, _) => await postalCodeService.CalculatorExistsForCode(postalCode))
            .WithErrorCode(HttpStatusCode.BadRequest.ToString())
            .WithMessage("Invalid Postal code. Calculator not found.");

        RuleFor(x => x.Income)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(HttpStatusCode.BadRequest.ToString())
            .WithMessage("Invalid income amount. Negative income is not valid.");
    }
}

public sealed record CalculateRequest([property: JsonPropertyName("PostalCode")] string? PostalCode, [property: JsonPropertyName("Income")] decimal Income) : ICommand<CalculateResultDto>;
public sealed record CalculateResultDto([property: JsonPropertyName("Calculator")] string Calculator, [property: JsonPropertyName("Tax")] decimal Tax);

public sealed class CalculateRequestHandler : ICommandHandler<CalculateRequest, CalculateResultDto>
{
    private readonly ICalculationService _calculationService;
    public CalculateRequestHandler(ICalculationService calculationService) => _calculationService = calculationService;
    public async ValueTask<CalculateResultDto> Handle(CalculateRequest request, CancellationToken cancellationToken) => await _calculationService.CalculateAsync(request, cancellationToken);
}