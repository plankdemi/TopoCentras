using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Core.Services;

public class UzsakymasService : IUzsakymasService
{
    private readonly IKlientasRepository _klientasRepository;
    private readonly IPrekeRepository _prekeRepository;

    private readonly IUzsakymasPrekeRepository _uzsakymasPrekeRepository;
    private readonly IUzsakymasRepository _uzsakymasRepository;


    public UzsakymasService(
        IUzsakymasRepository uzsakymasRepository,
        IPrekeRepository prekeRepository,
        IKlientasRepository klientasRepository,
        IUzsakymasPrekeRepository uzsakymasPrekeRepository)
    {
        _uzsakymasRepository = uzsakymasRepository;
        _prekeRepository = prekeRepository;
        _klientasRepository = klientasRepository;
        _uzsakymasPrekeRepository = uzsakymasPrekeRepository;
    }

    public Task<Uzsakymas?> GetUzsakymasByIdAsync(Guid id)
    {
        return _uzsakymasRepository.GetByIdAsync(id);
    }

    public Task<List<Uzsakymas>> GetAllUzsakymasAsync()
    {
        return _uzsakymasRepository.GetAllAsync();
    }

    public async Task CreateUzsakymasAsync(Guid klientasId, Dictionary<Guid, int> prekeKiekiai)
    {
        var bendraUzsakymoSuma = 0m;

        var klientas = await _klientasRepository.GetByIdAsync(klientasId)
                       ?? throw new InvalidOperationException("Klientas nerastas");

        var uzsakymas = new Uzsakymas
        {
            KlientasId = klientasId
        };

        foreach (var (prekeId, kiekis) in prekeKiekiai)
        {
            if (kiekis <= 0)
                throw new ArgumentException("Kiekis turi buti didesnis uz nuli.", nameof(prekeKiekiai));

            var preke = await _prekeRepository.GetByIdAsync(prekeId)
                        ?? throw new InvalidOperationException("Preke nerasta");

            uzsakymas.UzsakymasPrekes.Add(new UzsakymasPreke
            {
                UzsakymasId = uzsakymas.UzsakymasId,
                PrekeId = prekeId,
                Kiekis = kiekis
            });

            bendraUzsakymoSuma += preke.Kaina * kiekis;
        }

        uzsakymas.BendraUzsakymoSuma = bendraUzsakymoSuma;

        await _uzsakymasRepository.AddAsync(uzsakymas);
    }


    public async Task UpdateUzsakymasAsync(
        Guid uzsakymasId,
        Guid klientasId,
        Dictionary<Guid, int> prekeKiekiai)
    {
        var uzsakymas = await _uzsakymasRepository.GetByIdAsync(uzsakymasId)
                        ?? throw new InvalidOperationException("Uzsakymas nerastas");

        var klientas = await _klientasRepository.GetByIdAsync(klientasId)
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
                var preke = await _prekeRepository.GetByIdAsync(prekeId)
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

        foreach (var item in toRemove) uzsakymas.UzsakymasPrekes.Remove(item);


        uzsakymas.BendraUzsakymoSuma = 0m;
        foreach (var up in uzsakymas.UzsakymasPrekes) uzsakymas.BendraUzsakymoSuma += up.Preke.Kaina * up.Kiekis;

        await _uzsakymasRepository.UpdateAsync(uzsakymas);
    }

    public Task DeleteUzsakymasAsync(Guid id)
    {
        return _uzsakymasRepository.DeleteAsync(id);
    }

    public async Task<HashSet<Guid>> GetKlientasIdsWithUzsakymaiAsync()
    {
        var orders = await _uzsakymasRepository.GetAllAsync();
        return orders
            .Select(u => u.KlientasId)
            .ToHashSet();
    }

    public async Task<HashSet<Guid>> GetPrekeIdsWithUzsakymaiAsync()
    {
        var orders = await _uzsakymasRepository.GetAllAsync();
        return orders
            .SelectMany(u => u.UzsakymasPrekes)
            .Select(up => up.PrekeId)
            .ToHashSet();
    }
}