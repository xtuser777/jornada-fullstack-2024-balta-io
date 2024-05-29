using FinaFlow.Api.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Categories;
using FinaFlow.Core.Responses;

namespace FinaFlow.Api.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
        .WithName("Categories: delete")
        .WithSummary("Remove uma categoria")
        .WithDescription("Remove uma categoria")
        .WithOrder(3)
        .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        long id
        )
    {
        var request = new DeleteCategoryRequest()
        {
            Id = id,
            UserId = ApiConfiguration.UserId,
        };

        var result = await handler.DeleteAsync( request );
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
