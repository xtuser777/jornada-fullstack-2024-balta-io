using FinaFlow.Api.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Categories;
using FinaFlow.Core.Responses;

namespace FinaFlow.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
        .WithName("Categories: get by id")
        .WithSummary("Recupera uma categoria")
        .WithDescription("Recupera uma categoria")
        .WithOrder(4)
        .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        long id
        )
    {
        var request = new GetCategoryByIdRequest()
        {
            Id = id,
            UserId = ApiConfiguration.UserId,
        };

        var result = await handler.GetByIdAsync( request );

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
