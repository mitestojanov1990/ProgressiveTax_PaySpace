using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services.Abstractions;

public interface IHistoryService
{
    Task<List<CalculatorHistoryDto>> GetHistoryAsync();
}