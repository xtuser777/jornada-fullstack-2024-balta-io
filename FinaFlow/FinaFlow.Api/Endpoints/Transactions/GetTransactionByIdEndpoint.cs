using FinaFlow.Api.Common.Api;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Transactions;
using FinaFlow.Core.Responses;

namespace FinaFlow.Api.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Transactions: Get By Id")
            .WithSummary("Recupera uma transação")
            .WithDescription("Recupera uma transação")
            .WithOrder(4)
            .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        long id)
    {
        var request = new GetTransactionByIdRequest
        {
            UserId = ApiConfiguration.UserId,
            Id = id
        };

        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
