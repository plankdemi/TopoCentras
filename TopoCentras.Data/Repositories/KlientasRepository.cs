using Microsoft.EntityFrameworkCore;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Data.Repositories;

public class KlientasRepository : IKlientasRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public KlientasRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Klientas?> GetByIdAsync(Guid id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Klientai.FindAsync(id);
    }

    public async Task<List<Klientas>> GetAllAsync()
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Klientai.AsNoTracking().ToListAsync();
    }


    public async Task AddAsync(Klientas klientas)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Klientai.Add(klientas);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Klientas klientas)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        var existing = await dbContext.Klientai.FindAsync(klientas.KlientasId);
        if (existing != null)
        {
            existing.Pavadinimas = klientas.Pavadinimas;
        }


        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        var klientas = await dbContext.Klientai.FindAsync(id) ??
                       throw new InvalidOperationException("Klientas Nerastas");
        dbContext.Klientai.Remove(klientas);
        await dbContext.SaveChangesAsync();
    }
}