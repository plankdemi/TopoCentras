using TopoCentras.Core.Models;

namespace TopoCentras.Core.Interfaces;

public interface IPrekeRepository
{
    Task<Preke?> GetByIdAsync(Guid id);
    Task<List<Preke>> GetAllAsync();

    Task AddAsync(Preke preke);
    Task UpdateAsync(Preke preke);
    Task DeleteAsync(Guid id);
}