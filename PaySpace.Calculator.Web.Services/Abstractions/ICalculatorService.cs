using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services.Abstractions;

public interface ICalculatorService
{
    Task<CalculateResultDto> CalculateTaxAsync(CalculateRequest calculationRequest);
}