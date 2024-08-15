namespace PaySpace.Calculator.Web.Services.Abstractions;
public interface ISessionManager
{
    void SetToken(string token);
    void ClearToken();
    string? GetToken();
}