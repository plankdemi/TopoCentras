using Microsoft.EntityFrameworkCore;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Data.Repositories;

public class PrekeRepository : IPrekeRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public PrekeRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


    public async Task<Preke?> GetByIdAsync(Guid id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Prekes.FindAsync(id).AsTask();
    }

    public async Task<List<Preke>> GetAllAsync()
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Prekes.AsNoTracking().ToListAsync();
    }


    public async Task AddAsync(Preke preke)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Prekes.Add(preke);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Preke preke)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        var existing = await dbContext.Prekes.FindAsync(preke.PrekeId);
        if (existing != null)
        {
            existing.Pavadinimas = preke.Pavadinimas;
            existing.Gamintojas = preke.Gamintojas;
            existing.Kaina = preke.Kaina;
            
        }
        
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        var preke = await dbContext.Prekes.FindAsync(id) ?? throw new InvalidOperationException("Preke Nerasta");
        dbContext.Prekes.Remove(preke);
        await dbContext.SaveChangesAsync();
    }
}