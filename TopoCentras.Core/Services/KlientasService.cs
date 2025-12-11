using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;

namespace TopoCentras.Core.Services;

public class KlientasService : IKlientasService
{
    private readonly IKlientasRepository _klientasRepository;

    public KlientasService(IKlientasRepository klientasRepository)
    {
        _klientasRepository = klientasRepository;
    }


    public Task<Klientas?> GetKlientasByIdAsync(Guid id)
    {
        return _klientasRepository.GetByIdAsync(id);
    }

    public Task<List<Klientas>> GetAllKlientasAsync()
    {
        return _klientasRepository.GetAllAsync();
    }

    public async Task CreateKlientasAsync(string pavadinimas)
    {
        if (string.IsNullOrWhiteSpace(pavadinimas))
            throw new ArgumentException("Pavadinimas privalomas", nameof(pavadinimas));

        var klientas = new Klientas
        {
            Pavadinimas = pavadinimas.Trim()
        };
        await _klientasRepository.AddAsync(klientas);
    }

    public Task UpdateKlientasAsync(Klientas klientas)
    {
        return _klientasRepository.UpdateAsync(klientas);
    }

    public Task DeleteKlientasAsync(Guid id)
    {
        return _klientasRepository.DeleteAsync(id);
    }
}