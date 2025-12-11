using System.ComponentModel.DataAnnotations;

namespace TopoCentras.Core.Models;

public class UzsakymasPreke
{
    public Guid UzsakymasId { get; set; }
    public Uzsakymas Uzsakymas { get; set; } = null!;
    public Guid PrekeId { get; set; }

    [Required] public Preke Preke { get; set; } = null!;

    [Range(1, int.MaxValue)] public int Kiekis { get; set; }
}