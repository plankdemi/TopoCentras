using TopoCentras.Core.Models;

namespace TopoCentras.Core.Interfaces;

public interface IUzsakymasPrekeService
{
    Task<UzsakymasPreke?> GetUzsakymasPrekeByIdAsync(Guid uzsakymasId, Guid prekeId);

    Task CreateUzsakymasPrekeAsync(Guid uzsakymasId, Guid prekeId, int kiekis);
    Task UpdateUzsakymasPrekeAsync(UzsakymasPreke uzsakymasPreke);
    Task DeleteUzsakymasPrekeAsync(Guid uzsakymasId, Guid prekeId);
}