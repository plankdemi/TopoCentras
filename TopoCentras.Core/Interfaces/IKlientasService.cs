using TopoCentras.Core.Models;

namespace TopoCentras.Core.Interfaces;

public interface IKlientasService
{
    Task<Klientas?> GetKlientasByIdAsync(Guid id);
    Task<List<Klientas>> GetAllKlientasAsync();

    Task CreateKlientasAsync(string pavadinimas);
    Task UpdateKlientasAsync(Klientas klientas);
    Task DeleteKlientasAsync(Guid id);
}