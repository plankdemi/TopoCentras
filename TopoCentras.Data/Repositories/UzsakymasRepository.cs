using Microsoft.EntityFrameworkCore;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Data.Repositories;

public class UzsakymasRepository : IUzsakymasRepository
{
    private readonly AppDbContext _dbContext;

    public UzsakymasRepository(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }


    public Task<Uzsakymas?> GetByIdAsync(Guid id)
    {
        return _dbContext.Uzsakymai
            .Include(u => u.UzsakymasPrekes)
            .ThenInclude(up => up.Preke)
            .Include(u => u.Klientas)
            .FirstOrDefaultAsync(u => u.UzsakymasId == id);
    }

    public Task<List<Uzsakymas>> GetAllAsync()
    {
        return _dbContext.Uzsakymai
            .Include(u => u.Klientas)
            .Include(u => u.UzsakymasPrekes)
            .ThenInclude(up => up.Preke)
            .AsNoTracking()
            .ToListAsync();
    }


    public async Task AddAsync(Uzsakymas uzsakymas)
    {
        _dbContext.Uzsakymai.Add(uzsakymas);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Uzsakymas uzsakymas)
    {
        _dbContext.Uzsakymai.Update(uzsakymas);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var uzsakymas = await _dbContext.Uzsakymai.FindAsync(id) ??
                        throw new InvalidOperationException("Uzsakymas Nerastas");
        _dbContext.Uzsakymai.Remove(uzsakymas);
        await _dbContext.SaveChangesAsync();
    }
}