using Insurance.Domain.Entities.Contratacao;
using Insurance.Domain.Entities.Proposta;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Infra.Persistence
{
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
            : base(options) { }

        public DbSet<Proposta> Propostas { get; set; } = null!;
        public DbSet<Contratacao> Contratacoes { get; set; } = null!;

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

            modelBuilder.Entity<Contratacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PropostaId).IsRequired();
                entity.Property(e => e.DataContratacao).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}
