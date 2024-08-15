using Microsoft.AspNetCore.Http;
using PaySpace.Calculator.Web.Services.Abstractions;
using System.Text;

namespace PaySpace.Calculator.Web.Services;
public class SessionManager : ISessionManager
{
    private const string TokenKey = "JWToken";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetToken(string token)
    {
        var session = GetSession();
        var tokenBytes = Encoding.UTF8.GetBytes(token);
        session.Set(TokenKey, tokenBytes);
    }
    public void ClearToken()
    {
        var session = GetSession();
        session.Remove(TokenKey);
    }

    public string? GetToken()
    {
        var session = GetSession();
        if (session.TryGetValue(TokenKey, out byte[] tokenBytes))
        {
            return Encoding.UTF8.GetString(tokenBytes);
        }
        return null;
    }
    private ISession GetSession()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null)
        {
            throw new InvalidOperationException("Session is not available.");
        }
        return session;
    }
}