using Insurance.Domain.Entities.Proposta;
using Microsoft.EntityFrameworkCore;

namespace Insurance.PropostaService.Infra.Persistence;

public class PropostaDbContext : DbContext
{
    public PropostaDbContext(DbContextOptions<PropostaDbContext> options)
        : base(options) { }

    public DbSet<Proposta> Propostas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Proposta>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cliente).IsRequired();
            entity.Property(e => e.Valor).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}