using Insurance.Domain.Entities.Contratacao;
using Microsoft.EntityFrameworkCore;

namespace Insurance.ContratacaoService.Infra.Persistence; 

public class ContratacaoDbContext : DbContext
{
    public ContratacaoDbContext(DbContextOptions<ContratacaoDbContext> options)
        : base(options) { }

    public DbSet<Contratacao> Contratacoes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contratacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PropostaId).IsRequired();
            entity.Property(e => e.DataContratacao).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}