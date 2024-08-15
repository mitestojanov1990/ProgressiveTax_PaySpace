using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services;

public sealed class CalculatorHttpService : ICalculatorService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    public CalculatorHttpService(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }
    public async Task<List<PostalCodeDto>> GetPostalCodesAsync()
    {
        SetAuthorizationHeader();
        var response = await _httpClient.GetAsync("api/postalcode");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Cannot fetch postal codes, status code: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<List<PostalCodeDto>>() ?? [];
    }

    public async Task<List<CalculatorHistoryDto>> GetHistoryAsync()
    {
        SetAuthorizationHeader();
        var response = await _httpClient.GetAsync("api/history");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Cannot fetch history, status code: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<List<CalculatorHistoryDto>>() ?? [];
    }

    public async Task<CalculateResultDto> CalculateTaxAsync(CalculateRequest calculationRequest)
    {
        SetAuthorizationHeader();
        JsonContent content = JsonContent.Create(calculationRequest);
        var response = await _httpClient.PostAsync("api/calculate-tax", content);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Calculation was not executed, status code: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<CalculateResultDto>() ?? null;
    }

    public async Task<HttpResponseMessage> LoginAsync(HttpContent content)
    {
        var response = await _httpClient.PostAsync("login", content);
        return response;
    }

    private void SetAuthorizationHeader()
    {
        _tokenService.SetAuthorizationHeader(_httpClient);
    }
}