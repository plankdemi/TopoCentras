using TopoCentras.Core.Models;

namespace TopoCentras.Core.Interfaces;

public interface IUzsakymasPrekeRepository
{
    Task<UzsakymasPreke?> GetByIdAsync(Guid uzsakymasId, Guid prekeId);

    Task AddAsync(UzsakymasPreke uzsakymasPreke);
    Task UpdateAsync(UzsakymasPreke uzsakymasPreke);
    Task DeleteAsync(Guid uzsakymasId, Guid prekeId);
}