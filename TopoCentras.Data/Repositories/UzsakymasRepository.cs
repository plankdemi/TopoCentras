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

        if (uzsakymas.UzsakymasPrekes.Any())
        {
            dbContext.UzsakymoPrekes.RemoveRange(uzsakymas.UzsakymasPrekes);
            uzsakymas.UzsakymasPrekes.Clear();
        }


        if (!prekeKiekiai.Any())
        {
            uzsakymas.BendraUzsakymoSuma = 0m;
            await dbContext.SaveChangesAsync();
            return;
        }


        var prekeIds = prekeKiekiai.Keys.ToList();

        var prekes = await dbContext.Prekes
            .Where(p => prekeIds.Contains(p.PrekeId))
            .ToListAsync();

        var prekesById = prekes.ToDictionary(p => p.PrekeId);


        decimal total = 0m;

        foreach (var (prekeId, kiekis) in prekeKiekiai)
        {
            if (kiekis <= 0)
                continue;

            if (!prekesById.TryGetValue(prekeId, out var preke))
                throw new InvalidOperationException("Preke nerasta");

            var line = new UzsakymasPreke
            {
                UzsakymasId = uzsakymas.UzsakymasId,
                PrekeId = prekeId,
                Kiekis = kiekis
            };

            uzsakymas.UzsakymasPrekes.Add(line);
            total += preke.Kaina * kiekis;
        }

        uzsakymas.BendraUzsakymoSuma = total;

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