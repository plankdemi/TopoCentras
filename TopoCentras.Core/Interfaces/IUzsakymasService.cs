using TopoCentras.Core.Models;

namespace TopoCentras.Core.Interfaces;

public interface IUzsakymasService
{
    Task<Uzsakymas?> GetUzsakymasByIdAsync(Guid id);
    Task<List<Uzsakymas>> GetAllUzsakymasAsync();

    Task CreateUzsakymasAsync(Guid klientasId, Dictionary<Guid, int> prekeKiekiai);
    Task UpdateUzsakymasAsync(Guid uzsakymasId, Guid klientasId, Dictionary<Guid, int> prekeKiekiai);
    Task DeleteUzsakymasAsync(Guid id);

    Task<HashSet<Guid>> GetKlientasIdsWithUzsakymaiAsync();
    Task<HashSet<Guid>> GetPrekeIdsWithUzsakymaiAsync();
}