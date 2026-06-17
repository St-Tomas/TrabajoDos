using Microsoft.EntityFrameworkCore;
using MyControllerApi.Models;

namespace MyControllerApi.Data;

public class BancoDbContext : DbContext
{
    public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options)
    {
    }

    public DbSet<Persona> Personas { get; set; }
    public DbSet<Cuenta> Cuentas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar la tabla Personas
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Dni).IsRequired();
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Dni).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configurar la tabla Cuentas
        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Tipo).IsRequired();
            entity.Property(e => e.Saldo).HasPrecision(18, 2);
            entity.Property(e => e.PersonaId).IsRequired();
            entity.HasIndex(e => e.PersonaId);
        });
    }
}
