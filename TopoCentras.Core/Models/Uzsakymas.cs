using System.ComponentModel.DataAnnotations;

namespace TopoCentras.Core.Models;

public class Uzsakymas
{
    [Key] public Guid UzsakymasId { get; set; } = Guid.NewGuid();

    public Guid KlientasId { get; set; }
    public Klientas Klientas { get; set; } = null!;
    public ICollection<UzsakymasPreke> UzsakymasPrekes { get; set; } = new List<UzsakymasPreke>();

    public decimal BendraUzsakymoSuma { get; set; }
    public DateTime UzsakymoSukurimoData { get; set; } = DateTime.UtcNow;
}