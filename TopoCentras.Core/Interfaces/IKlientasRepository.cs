using TopoCentras.Core.Models;

namespace TopoCentras.Core.Interfaces;

public interface IKlientasRepository
{
    Task<Klientas?> GetByIdAsync(Guid id);
    Task<List<Klientas>> GetAllAsync();

    Task AddAsync(Klientas klientas);
    Task UpdateAsync(Klientas klientas);
    Task DeleteAsync(Guid id);
}