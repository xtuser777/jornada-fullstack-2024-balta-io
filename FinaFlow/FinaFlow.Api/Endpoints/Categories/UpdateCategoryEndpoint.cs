using FinaFlow.Api.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Categories;
using FinaFlow.Core.Responses;

namespace FinaFlow.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
        .WithName("Categories: update")
        .WithSummary("Atualiza uma categoria")
        .WithDescription("Atualiza uma categoria")
        .WithOrder(2)
        .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        UpdateCategoryRequest request,
        long id
    )
    {
        request.UserId = ApiConfiguration.UserId;
        request.Id = id;

        var result = await handler.UpdateAsync( request );

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
