using Microsoft.EntityFrameworkCore;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Data.Repositories;

public class UzsakymasPrekeRepository : IUzsakymasPrekeRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public UzsakymasPrekeRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


    public async Task<UzsakymasPreke?> GetByIdAsync(Guid uzsakymasId, Guid prekeId)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.UzsakymoPrekes.FindAsync(uzsakymasId, prekeId).AsTask();
    }


    public async Task AddAsync(UzsakymasPreke uzsakymasPreke)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.UzsakymoPrekes.Add(uzsakymasPreke);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(UzsakymasPreke uzsakymasPreke)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.UzsakymoPrekes.Update(uzsakymasPreke);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid uzsakymasId, Guid prekeId)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        var uzsakymasPreke = await dbContext.UzsakymoPrekes.FindAsync(uzsakymasId, prekeId) ??
                             throw new InvalidOperationException("Uzsakymo Preke Nerasta");
        dbContext.UzsakymoPrekes.Remove(uzsakymasPreke);
        await dbContext.SaveChangesAsync();
    }
}