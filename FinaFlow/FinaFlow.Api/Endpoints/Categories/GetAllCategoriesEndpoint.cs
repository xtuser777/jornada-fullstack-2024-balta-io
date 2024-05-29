using FinaFlow.Api.Common.Api;
using FinaFlow.Core;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Categories;
using FinaFlow.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinaFlow.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
        .WithName("Categories: get all")
        .WithSummary("Recupera todas categorias")
        .WithDescription("Recupera todas categorias")
        .WithOrder(5)
        .Produces<PagedResponse<List<Category>?>>();

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize
    )
    {
        var request = new GetAllCategoriesRequest()
        {
            UserId = ApiConfiguration.UserId,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await handler.GetAllAsync( request );

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
