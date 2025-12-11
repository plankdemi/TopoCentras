using TopoCentras.Core.Models;

namespace TopoCentras.Core.Interfaces;

public interface IUzsakymasRepository
{
    Task<Uzsakymas?> GetByIdAsync(Guid id);
    Task<List<Uzsakymas>> GetAllAsync();

    Task AddAsync(Uzsakymas uzsakymas);
    Task UpdateAsync(Uzsakymas uzsakymas);
    Task DeleteAsync(Guid id);
}