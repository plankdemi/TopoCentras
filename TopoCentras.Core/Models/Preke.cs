using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopoCentras.Core.Models;

public class Preke
{
    [Key] public Guid PrekeId { get; set; } = Guid.NewGuid();

    [Required] [MaxLength(255)] public string Pavadinimas { get; set; } = null!;

    [Required] [MaxLength(255)] public string Gamintojas { get; set; } = null!;

    public decimal Kaina { get; set; }

    [NotMapped] public bool CanDelete { get; set; } = true;
}