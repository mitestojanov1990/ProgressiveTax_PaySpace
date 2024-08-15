using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services.Abstractions;

public interface IAuthService
{
    Task<HttpResponseMessage> LoginAsync(HttpContent content);
}