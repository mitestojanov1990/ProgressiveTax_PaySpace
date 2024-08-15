using System.Net.Http.Json;
using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services;
public sealed class CalculatorService : ICalculatorService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    public CalculatorService(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    public async Task<CalculateResultDto> CalculateTaxAsync(CalculateRequest calculationRequest)
    {
        _tokenService.SetAuthorizationHeader(_httpClient);
        JsonContent content = JsonContent.Create(calculationRequest);
        var response = await _httpClient.PostAsync("api/calculate-tax", content);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Calculation was not executed, status code: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<CalculateResultDto>() ?? throw new InvalidOperationException("Response is null");
    }
}