using Insurance.ContratacaoService.Infra.Persistence;
using Insurance.Domain.Entities.Contratacao;
using Insurance.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Insurance.ContratacaoService.Api.Tests.Repositories
{
    public class ContratacaoRepositoryTests
    {
        private ContratacaoDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ContratacaoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ContratacaoDbContext(options);
        }

        [Fact]
        public async Task AddAsync_DeveAdicionarContratacao()
        {
            var context = CreateDbContext();
            var repo = new ContratacaoRepository(context);

            var contratacao = new Contratacao(Guid.NewGuid());
            await repo.AddAsync(contratacao);

            var saved = await context.Contratacoes.FirstOrDefaultAsync();
            Assert.NotNull(saved);
            Assert.Equal(contratacao.PropostaId, saved.PropostaId);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarContratacao()
        {
            var context = CreateDbContext();
            var repo = new ContratacaoRepository(context);

            var contratacao = new Contratacao(Guid.NewGuid());
            await repo.AddAsync(contratacao);

            var result = await repo.GetByIdAsync(contratacao.Id);

            Assert.NotNull(result);
            Assert.Equal(contratacao.Id, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarTodasContratacoes()
        {
            var context = CreateDbContext();
            var repo = new ContratacaoRepository(context);

            await repo.AddAsync(new Contratacao(Guid.NewGuid()));
            await repo.AddAsync(new Contratacao(Guid.NewGuid()));

            var result = await repo.GetAllAsync();
            Assert.Equal(2, result.Count());
        }
    }
}
