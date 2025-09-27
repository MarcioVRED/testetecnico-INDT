using Insurance.Domain.Entities.Proposta;
using Insurance.Infra.Persistence;
using Insurance.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Insurance.PropostaService.Api.Tests.Repositories;

public class PropostaRepositoryTests
{
    private InsuranceDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InsuranceDbContext(options);
    }

    [Fact]
    public async Task AddAsync_DeveAdicionarProposta()
    {
        var context = CreateDbContext();
        var repo = new PropostaRepository(context);

        var proposta = new Proposta("Cliente Teste", 1000);

        await repo.AddAsync(proposta);

        var saved = await context.Propostas.FirstOrDefaultAsync();
        Assert.NotNull(saved);
        Assert.Equal(proposta.Cliente, saved.Cliente);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarProposta()
    {
        var context = CreateDbContext();
        var repo = new PropostaRepository(context);

        var proposta = new Proposta("Cliente Teste", 500);
        await repo.AddAsync(proposta);

        var result = await repo.GetByIdAsync(proposta.Id);

        Assert.NotNull(result);
        Assert.Equal(proposta.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodasPropostas()
    {
        var context = CreateDbContext();
        var repo = new PropostaRepository(context);

        await repo.AddAsync(new Proposta("Cliente1", 100));
        await repo.AddAsync(new Proposta("Cliente2", 200));

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarProposta()
    {
        var context = CreateDbContext();
        var repo = new PropostaRepository(context);

        var proposta = new Proposta("Cliente Original", 100);
        await repo.AddAsync(proposta);

        var valorProp = typeof(Proposta).GetProperty("Valor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        valorProp?.SetValue(proposta, 200m);
        await repo.UpdateAsync(proposta);

        var result = await repo.GetByIdAsync(proposta.Id);
        Assert.Equal(200, result?.Valor);
    }
}
