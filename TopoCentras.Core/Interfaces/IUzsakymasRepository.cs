using TopoCentras.Core.Models;

namespace TopoCentras.Core.Interfaces;

public interface IUzsakymasRepository
{
    Task<Uzsakymas?> GetByIdAsync(Guid id);
    Task<List<Uzsakymas>> GetAllAsync();

    Task AddAsync(Uzsakymas uzsakymas);
    Task UpdateAsync(Guid uzsakymasId, Guid klientasId, Dictionary<Guid, int> prekeKiekiai);
    Task DeleteAsync(Guid id);
}