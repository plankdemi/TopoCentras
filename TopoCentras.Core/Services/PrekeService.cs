using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Core.Services;

public class PrekeService : IPrekeService
{
    private readonly IPrekeRepository _prekeRepository;

    public PrekeService(IPrekeRepository prekeRepository)
    {
        _prekeRepository = prekeRepository;
    }


    public Task<Preke?> GetPrekeByIdAsync(Guid id)
    {
        return _prekeRepository.GetByIdAsync(id);
    }

    public Task<List<Preke>> GetAllPrekeAsync()
    {
        return _prekeRepository.GetAllAsync();
    }


    public async Task CreatePrekeAsync(string pavadinimas, string gamintojas, decimal kaina)
    {
        if (string.IsNullOrWhiteSpace(pavadinimas))
            throw new ArgumentException("Pavadinimas privalomas", nameof(pavadinimas));

        if (string.IsNullOrWhiteSpace(gamintojas))
            throw new ArgumentException("Gamintojas privalomas", nameof(gamintojas));

        if (kaina < 0) throw new ArgumentException("Kaina negali buti neeigiama", nameof(kaina));

        var preke = new Preke
        {
            Pavadinimas = pavadinimas.Trim(),
            Gamintojas = gamintojas.Trim(),
            Kaina = kaina
        };
        await _prekeRepository.AddAsync(preke);
    }

    public Task UpdatePrekeAsync(Preke preke)
    {
        return _prekeRepository.UpdateAsync(preke);
    }

    public Task DeletePrekeAsync(Guid id)
    {
        return _prekeRepository.DeleteAsync(id);
    }
}