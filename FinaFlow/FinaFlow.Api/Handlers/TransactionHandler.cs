using FinaFlow.Api.Data;
using FinaFlow.Core.Common;
using FinaFlow.Core.Enums;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Transactions;
using FinaFlow.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinaFlow.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;

        try
        {
            var transaction = new Transaction()
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 201, "Transação cadastrada com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Transação não cadastrada com sucesso!");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;

        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada!");

            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, message: "Transação atualizada com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Transação não atualizada com sucesso!");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada!");

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, message: "Transação removida com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Transação não removida com sucesso!");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            return transaction is null
                ? new Response<Transaction?>(null, 404, "Transação não encontrada!")
                : new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Transação não recuperada com sucesso!");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Datas de inicio e fim invalidas.");
        }

        try
        {
            var query = context
            .Transactions
            .AsNoTracking()
            .Where(c => c.PaidOrReceivedAt >= request.StartDate &&
                        c.PaidOrReceivedAt <= request.EndDate &&
                        c.UserId == request.UserId)
            .OrderBy(c => c.PaidOrReceivedAt);

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(
                transactions,
                count,
                request.PageNumber,
                request.PageSize
            );
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Transações não recuperadas com sucesso.");
        }
    }
}
