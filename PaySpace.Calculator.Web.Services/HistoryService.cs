using System.Net.Http.Json;
using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services;

public sealed class HistoryService : IHistoryService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    public HistoryService(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    public async Task<List<CalculatorHistoryDto>> GetHistoryAsync()
    {
        _tokenService.SetAuthorizationHeader(_httpClient);
        var response = await _httpClient.GetAsync("api/history");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Cannot fetch history, status code: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<List<CalculatorHistoryDto>>() ?? new List<CalculatorHistoryDto>();
    }
}