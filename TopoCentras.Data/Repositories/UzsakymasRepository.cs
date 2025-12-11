using Microsoft.EntityFrameworkCore;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Data.Repositories;

public class UzsakymasRepository : IUzsakymasRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public UzsakymasRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


    public async Task<Uzsakymas?> GetByIdAsync(Guid id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Uzsakymai
            .Include(u => u.UzsakymasPrekes)
            .ThenInclude(up => up.Preke)
            .Include(u => u.Klientas)
            .FirstOrDefaultAsync(u => u.UzsakymasId == id);
    }

    public async Task<List<Uzsakymas>> GetAllAsync()
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        return await dbContext.Uzsakymai
            .Include(u => u.Klientas)
            .Include(u => u.UzsakymasPrekes)
            .ThenInclude(up => up.Preke)
            .AsNoTracking()
            .ToListAsync();
    }


    public async Task AddAsync(Uzsakymas uzsakymas)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Uzsakymai.Add(uzsakymas);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Uzsakymas uzsakymas)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        dbContext.Uzsakymai.Update(uzsakymas);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        var uzsakymas = await dbContext.Uzsakymai.FindAsync(id) ??
                        throw new InvalidOperationException("Uzsakymas Nerastas");
        dbContext.Uzsakymai.Remove(uzsakymas);
        await dbContext.SaveChangesAsync();
    }
}