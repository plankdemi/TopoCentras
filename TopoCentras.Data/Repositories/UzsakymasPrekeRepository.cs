using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Data.Repositories;

public class UzsakymasPrekeRepository : IUzsakymasPrekeRepository
{
    private readonly AppDbContext _dbContext;

    public UzsakymasPrekeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Task<UzsakymasPreke?> GetByIdAsync(Guid uzsakymasId, Guid prekeId)
    {
        return _dbContext.UzsakymoPrekes.FindAsync(uzsakymasId, prekeId).AsTask();
    }


    public async Task AddAsync(UzsakymasPreke uzsakymasPreke)
    {
        _dbContext.UzsakymoPrekes.Add(uzsakymasPreke);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(UzsakymasPreke uzsakymasPreke)
    {
        _dbContext.UzsakymoPrekes.Update(uzsakymasPreke);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid uzsakymasId, Guid prekeId)
    {
        var uzsakymasPreke = await _dbContext.UzsakymoPrekes.FindAsync(uzsakymasId, prekeId) ??
                             throw new InvalidOperationException("Uzsakymo Preke Nerasta");
        _dbContext.UzsakymoPrekes.Remove(uzsakymasPreke);
        await _dbContext.SaveChangesAsync();
    }
}