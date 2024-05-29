using FinaFlow.Api.Data;
using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Categories;
using FinaFlow.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinaFlow.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category()
        {
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description
        };

        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 201, "Categoria cadastrada com sucesso!");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Categoria não cadastrada com sucesso!");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(null, 404, "Categoria não encontrada!");

            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria atualizada com sucesso!");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Categoria não atualizada com sucesso!");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(null, 404, "Categoria não encontrada!");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria removida com sucesso!");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Categoria não removida com sucesso!");
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        var query = context
            .Categories
            .AsNoTracking()
            .Where(c => c.UserId == request.UserId)
            .OrderBy(c => c.Title);

        var categories = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var count = await query.CountAsync();

        return new PagedResponse<List<Category>?>(
            categories,
            count,
            request.PageNumber,
            request.PageSize
        );
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            return category is null
                ? new Response<Category?>(null, 404, "Categoria não encontrada!")
                : new Response<Category?>(category);
        }
        catch
        {
            return new Response<Category?>(null, 500, "Categoria não recuperada com sucesso!");
        }
    }
}
