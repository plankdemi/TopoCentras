using Microsoft.EntityFrameworkCore;
using TopoCentras.Core.Models;

namespace TopoCentras.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Klientas> Klientai => Set<Klientas>();
    public DbSet<Preke> Prekes => Set<Preke>();
    public DbSet<Uzsakymas> Uzsakymai => Set<Uzsakymas>();
    public DbSet<UzsakymasPreke> UzsakymoPrekes => Set<UzsakymasPreke>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UzsakymasPreke>(entity =>
        {
            entity.HasKey(x => new
            {
                x.UzsakymasId,
                x.PrekeId
            });


            entity.HasOne(x => x.Uzsakymas)
                .WithMany(u => u.UzsakymasPrekes)
                .HasForeignKey(x => x.UzsakymasId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Uzsakymas>()
            .Property(u => u.BendraUzsakymoSuma)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Preke>()
            .Property(u => u.Kaina)
            .HasPrecision(18, 2);
    }
}