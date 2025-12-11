using Microsoft.EntityFrameworkCore;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Data.Repositories;

public class KlientasRepository : IKlientasRepository
{
    private readonly AppDbContext _dbContext;

    public KlientasRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Klientas?> GetByIdAsync(Guid id)
    {
        return _dbContext.Klientai.FindAsync(id).AsTask();
    }

    public Task<List<Klientas>> GetAllAsync()
    {
        return _dbContext.Klientai.AsNoTracking().ToListAsync();
    }


    public async Task AddAsync(Klientas klientas)
    {
        _dbContext.Klientai.Add(klientas);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Klientas klientas)
    {
        _dbContext.Klientai.Update(klientas);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var klientas = await _dbContext.Klientai.FindAsync(id) ??
                       throw new InvalidOperationException("Klientas Nerastas");
        _dbContext.Klientai.Remove(klientas);
        await _dbContext.SaveChangesAsync();
    }
}