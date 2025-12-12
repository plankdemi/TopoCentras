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

    public async Task UpdateAsync(
        Guid uzsakymasId,
        Guid klientasId,
        Dictionary<Guid, int> prekeKiekiai)
    {
        await using var dbContext = _dbContextFactory.CreateDbContext();
        
        var uzsakymas = await dbContext.Uzsakymai
                            .Include(u => u.UzsakymasPrekes)
                            .ThenInclude(up => up.Preke)
                            .FirstOrDefaultAsync(u => u.UzsakymasId == uzsakymasId)
                        ?? throw new InvalidOperationException("Uzsakymas nerastas");

        var klientas = await dbContext.Klientai
                           .FirstOrDefaultAsync(k => k.KlientasId == klientasId)
                       ?? throw new InvalidOperationException("Klientas nerastas");

        uzsakymas.KlientasId = klientasId;
        uzsakymas.Klientas = klientas;

        
        var existingByPrekeId = uzsakymas.UzsakymasPrekes
            .ToDictionary(up => up.PrekeId);

       
        foreach (var (prekeId, kiekis) in prekeKiekiai)
        {
            if (kiekis <= 0)
                throw new ArgumentException("Kiekis turi buti didesnis uz nuli.", nameof(prekeKiekiai));

            if (existingByPrekeId.TryGetValue(prekeId, out var existing))
            {
                existing.Kiekis = kiekis;
            }
            else
            {
                var preke = await dbContext.Prekes
                                .FirstOrDefaultAsync(p => p.PrekeId == prekeId)
                            ?? throw new InvalidOperationException("Preke nerasta");

                uzsakymas.UzsakymasPrekes.Add(new UzsakymasPreke
                {
                    UzsakymasId = uzsakymas.UzsakymasId,
                    PrekeId = prekeId,
                    Preke = preke,
                    Kiekis = kiekis
                });
            }
        }

        
        var toRemove = uzsakymas.UzsakymasPrekes
            .Where(up => !prekeKiekiai.ContainsKey(up.PrekeId))
            .ToList();

        foreach (var item in toRemove)
            dbContext.UzsakymoPrekes.Remove(item);

        
        uzsakymas.BendraUzsakymoSuma = uzsakymas.UzsakymasPrekes
            .Sum(up => up.Preke.Kaina * up.Kiekis);

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