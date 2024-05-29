﻿using FinaFlow.Api.Common;
using FinaFlow.Api.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Categories;
using FinaFlow.Core.Responses;

namespace FinaFlow.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
        .WithName("Categories: create")
        .WithSummary("Cria uma nova categoria")
        .WithDescription("Cria uma nova categoria")
        .WithOrder(1)
        .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        CreateCategoryRequest request
    )
    {
        request.UserId = ApiConfiguration.UserId;
        var response = await handler.CreateAsync( request );

        return response.IsSuccess
            ? TypedResults.Created($"v1/categories/{response.Data?.Id}", response)
            : TypedResults.BadRequest(response);
    }
}
