using Microsoft.AspNetCore.Http;
using PaySpace.Calculator.Web.Services.Abstractions;
using System.Net.Http.Headers;

namespace PaySpace.Calculator.Web.Services;

public class TokenService : ITokenService
{
    private const string TokenKey = "JWToken";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetAuthorizationHeader(HttpClient httpClient)
    {
        _httpContextAccessor.HttpContext.Session.TryGetValue(TokenKey, out var tokenBytes);
        if (tokenBytes != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", System.Text.Encoding.UTF8.GetString(tokenBytes));
        }
    }
}