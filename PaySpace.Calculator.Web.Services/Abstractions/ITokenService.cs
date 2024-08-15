namespace PaySpace.Calculator.Web.Services.Abstractions; 

public interface ITokenService
{
    void SetAuthorizationHeader(HttpClient httpClient);
}
