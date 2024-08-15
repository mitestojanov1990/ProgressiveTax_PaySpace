using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PaySpace.Calculator.API;

public static class AuthApi
{
    public static WebApplication MapAuthApi(this WebApplication app)
    {
        app.MapPost(
            "login",
            async ([FromBody] LoginRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                if (request.Email == "dimitrycode@gmail.com" && request.Password == "!v3ryS3cur3d")
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenKey = Encoding.ASCII.GetBytes("your-very-secure-long-key-which-is-at-least-32-characters");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, request.Email)
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return Results.Ok(new { Token = tokenHandler.WriteToken(token) });
                }

                return Results.Unauthorized();
            }
        )
        .WithName("Login")
        .WithTags("Authentication")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);


        return app;
    }
}

public sealed record LoginRequest(string Email, string Password);