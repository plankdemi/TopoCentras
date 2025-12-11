using TopoCentras.Core.Models;

namespace TopoCentras.Core.Interfaces;

public interface IPrekeService
{
    Task<Preke?> GetPrekeByIdAsync(Guid id);
    Task<List<Preke>> GetAllPrekeAsync();

    Task CreatePrekeAsync(string pavadinimas, string gamintojas, decimal kaina);
    Task UpdatePrekeAsync(Preke preke);
    Task DeletePrekeAsync(Guid id);
}