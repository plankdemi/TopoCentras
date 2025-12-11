using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopoCentras.Core.Models;

public class Klientas
{
    [Key] public Guid KlientasId { get; set; } = Guid.NewGuid();

    [Required] [MaxLength(255)] public string Pavadinimas { get; set; } = null!;

    [NotMapped] public bool CanDelete { get; set; } = true;
}