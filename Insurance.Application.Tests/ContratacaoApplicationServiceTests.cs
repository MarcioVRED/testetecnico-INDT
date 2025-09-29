using Moq;
using Insurance.Application.Exceptions;
using Insurance.Domain.Repositories.Contracts;
using FluentAssertions;
using Insurance.Contratacao.Application.Services.Contracts;
using ContratacaoEntity = Insurance.Domain.Entities.Contratacao;

namespace Insurance.Application.Tests.Services;

public class ContratacaoApplicationServiceTests
{
    private readonly Mock<IContratacaoRepository> _contratacaoRepo = new();
    private readonly Mock<IPropostaStatusChecker> _propostaStatusChecker = new();
    private readonly ContratacaoApplicationService _service;

    public ContratacaoApplicationServiceTests()
    {
        _service = new ContratacaoApplicationService(_contratacaoRepo.Object, _propostaStatusChecker.Object);
    }

    [Fact]
    public async Task ContratarProposta_DeveCriarContratacao_QuandoPropostaAprovada()
    {
        var propostaId = Guid.NewGuid();
        _propostaStatusChecker.Setup(p => p.EhAprovadaAsync(propostaId)).ReturnsAsync(true);

        await _service.ContratarProposta(propostaId);

        _contratacaoRepo.Verify(r => r.AddAsync(It.IsAny<ContratacaoEntity>()), Times.Once);
    }

    [Fact]
    public async Task ContratarProposta_DeveLancarExcecao_QuandoPropostaNaoAprovada()
    {
        var propostaId = Guid.NewGuid();
        _propostaStatusChecker.Setup(p => p.EhAprovadaAsync(propostaId)).ReturnsAsync(false);

        Func<Task> act = async () => await _service.ContratarProposta(propostaId);

        await act.Should().ThrowAsync<PropostaNaoAprovadaException>();
        _contratacaoRepo.Verify(r => r.AddAsync(It.IsAny<ContratacaoEntity>()), Times.Never);
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarContratacao()
    {
        var contratacao = new ContratacaoEntity(Guid.NewGuid());
        _contratacaoRepo.Setup(r => r.GetByIdAsync(contratacao.Id)).ReturnsAsync(contratacao);

        var result = await _service.ObterPorId(contratacao.Id);

        result.Should().BeEquivalentTo(contratacao);
    }

    [Fact]
    public async Task ObterPorId_DeveLancarExcecao_QuandoNaoEncontrada()
    {
        _contratacaoRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ContratacaoEntity?)null);

        Func<Task> act = async () => await _service.ObterPorId(Guid.NewGuid());

        await act.Should().ThrowAsync<ContratacaoNaoEncontradaException>();
    }

    [Fact]
    public async Task ListarContratacoes_DeveRetornarLista()
    {
        var list = new List<ContratacaoEntity> { new(Guid.NewGuid()) };
        _contratacaoRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var result = await _service.ListarContratacoes();

        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(list);
    }

    [Fact]
    public async Task ListarContratacoes_DeveRetornarListaVazia_QuandoNaoExistirem()
    {
        _contratacaoRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ContratacaoEntity>());

        var result = await _service.ListarContratacoes();

        result.Should().BeEmpty();
    }
}