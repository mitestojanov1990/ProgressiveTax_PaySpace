using PaySpace.Calculator.Web.Services.Abstractions;

namespace PaySpace.Calculator.Web.Services;
public sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> LoginAsync(HttpContent content)
    {
        var response = await _httpClient.PostAsync("login", content);
        return response;
    }
}