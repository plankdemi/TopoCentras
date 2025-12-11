using Microsoft.EntityFrameworkCore;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Data.Repositories;

public class PrekeRepository : IPrekeRepository
{
    private readonly AppDbContext _dbContext;

    public PrekeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Task<Preke?> GetByIdAsync(Guid id)
    {
        return _dbContext.Prekes.FindAsync(id).AsTask();
    }

    public Task<List<Preke>> GetAllAsync()
    {
        return _dbContext.Prekes.AsNoTracking().ToListAsync();
    }


    public async Task AddAsync(Preke preke)
    {
        _dbContext.Prekes.Add(preke);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Preke preke)
    {
        _dbContext.Prekes.Update(preke);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var preke = await _dbContext.Prekes.FindAsync(id) ?? throw new InvalidOperationException("Preke Nerasta");
        _dbContext.Prekes.Remove(preke);
        await _dbContext.SaveChangesAsync();
    }
}