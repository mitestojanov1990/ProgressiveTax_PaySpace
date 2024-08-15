using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services.Abstractions;

public interface IPostalCodeService
{
    Task<List<PostalCodeDto>> GetPostalCodesAsync();
}