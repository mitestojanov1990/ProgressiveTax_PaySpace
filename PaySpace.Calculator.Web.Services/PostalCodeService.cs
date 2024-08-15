using System.Net.Http.Json;
using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services;

public sealed class PostalCodeService : IPostalCodeService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    public PostalCodeService(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    public async Task<List<PostalCodeDto>> GetPostalCodesAsync()
    {
        _tokenService.SetAuthorizationHeader(_httpClient);
        var response = await _httpClient.GetAsync("api/postalcode");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Cannot fetch postal codes, status code: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<List<PostalCodeDto>>() ?? new List<PostalCodeDto>();
    }
}