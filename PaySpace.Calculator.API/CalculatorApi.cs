using Mediator;
using Microsoft.AspNetCore.Mvc;
using PaySpace.Calculator.Application;

namespace PaySpace.Calculator.API;

public static class CalculatorApi
{
    public static WebApplication MapCalculatorApi(this WebApplication app)
    {
        var apiGroup = app.MapGroup("/api");

        apiGroup.MapPost(
            "calculate-tax",
            [Microsoft.AspNetCore.Authorization.Authorize] async ([FromBody] CalculateRequest request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                return Results.Ok(await mediator.Send(request, cancellationToken));
            }
        )
        .WithName("Calculate tax")
        .WithTags("Calculator")
        .Produces<CalculateResultDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status400BadRequest);

        apiGroup.MapGet(
            "history",
            [Microsoft.AspNetCore.Authorization.Authorize] async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                return Results.Ok(await mediator.Send(new GetCalculatorHistoryRequest()));
            }
        )
        .WithName("Get calculation history")
        .WithTags("Calculator")
        .Produces<CalculatorHistoryDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status400BadRequest);

        apiGroup.MapGet(
            "postalcode",
            [Microsoft.AspNetCore.Authorization.AllowAnonymous] async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                return Results.Ok(await mediator.Send(new GetPostalCodesRequest()));
            }
        )
        .WithName("Get postal codes")
        .WithTags("Calculator")
        .Produces<PostalCodeDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}
