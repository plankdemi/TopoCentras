using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Core.Services;

public class UzsakymasPrekeService : IUzsakymasPrekeService
{
    private readonly IPrekeRepository _prekeRepository;
    private readonly IUzsakymasPrekeRepository _uzsakymasPrekeRepository;
    private readonly IUzsakymasRepository _uzsakymasRepository;

    public UzsakymasPrekeService(IUzsakymasPrekeRepository uzsakymasPrekeRepository, IPrekeRepository prekeRepository,
        IUzsakymasRepository uzsakymasRepository)
    {
        _uzsakymasPrekeRepository = uzsakymasPrekeRepository;
        _prekeRepository = prekeRepository;
        _uzsakymasRepository = uzsakymasRepository;
    }

    public Task<UzsakymasPreke?> GetUzsakymasPrekeByIdAsync(Guid uzsakymasId, Guid prekeId)
    {
        return _uzsakymasPrekeRepository.GetByIdAsync(uzsakymasId, prekeId);
    }


    public async Task CreateUzsakymasPrekeAsync(Guid uzsakymasId, Guid prekeId, int kiekis)
    {
        if (kiekis <= 0) throw new ArgumentException("Kiekis turi buti didesnis uz nuli", nameof(kiekis));

        var preke = await _prekeRepository.GetByIdAsync(prekeId) ??
                    throw new InvalidOperationException("Preke Nerasta");
        var uzsakymas = await _uzsakymasRepository.GetByIdAsync(uzsakymasId) ??
                        throw new InvalidOperationException("Uzsakymas Nerastas");


        var uzsakymasPreke = new UzsakymasPreke
        {
            UzsakymasId = uzsakymasId,
            PrekeId = prekeId,
            Kiekis = kiekis
        };
        await _uzsakymasPrekeRepository.AddAsync(uzsakymasPreke);
    }

    public Task UpdateUzsakymasPrekeAsync(UzsakymasPreke uzsakymasPreke)
    {
        return _uzsakymasPrekeRepository.UpdateAsync(uzsakymasPreke);
    }

    public Task DeleteUzsakymasPrekeAsync(Guid uzsakymasId, Guid prekeId)
    {
        return _uzsakymasPrekeRepository.DeleteAsync(uzsakymasId, prekeId);
    }
}