using System.Net.Http.Json;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Transactions;
using FinaFlow.Core.Responses;
using FinaFlow.Web;

namespace FinaFlow.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);

    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
            ?? new Response<Transaction?>(null, 400, "Falha ao criar transação");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Falha ao atualizar a transação");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _client.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Falha ao excluir a transação");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}")
           ?? new Response<Transaction?>(null, 400, "Não foi possível obter a transação");

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
        => await _client.GetFromJsonAsync<PagedResponse<List<Transaction>?>>("v1/transactions")
           ?? new PagedResponse<List<Transaction>?>(null, 400, "Não foi possível obter as transações");
}