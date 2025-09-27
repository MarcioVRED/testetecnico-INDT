using Moq;
using Insurance.Application.Exceptions;
using Insurance.Domain.Entities.Contratacao;
using Insurance.Domain.Entities.Enums;
using Insurance.Domain.Entities.Proposta;
using Insurance.Domain.Repositories.Contracts;
using FluentAssertions;

namespace Insurance.Application.Tests.Services;

public class ContratacaoApplicationServiceTests
{
    private readonly Mock<IContratacaoRepository> _contratacaoRepo = new();
    private readonly Mock<IPropostaRepository> _propostaRepo = new();
    private readonly ContratacaoApplicationService _service;

    public ContratacaoApplicationServiceTests()
    {
        _service = new ContratacaoApplicationService(_contratacaoRepo.Object, _propostaRepo.Object);
    }

    [Fact]
    public async Task ContratarProposta_DeveCriarContratacao_QuandoPropostaAprovada()
    {
        var proposta = new Proposta("Cliente", 100);
        proposta.AlterarStatus(StatusProposta.Aprovada);

        _propostaRepo.Setup(r => r.GetByIdAsync(proposta.Id)).ReturnsAsync(proposta);

        await _service.ContratarProposta(proposta.Id);

        _contratacaoRepo.Verify(r => r.AddAsync(It.IsAny<Contratacao>()), Times.Once);
    }

    [Fact]
    public async Task ContratarProposta_DeveLancarExcecao_QuandoPropostaNaoEncontrada()
    {
        _propostaRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Proposta?)null);

        Func<Task> act = async () => await _service.ContratarProposta(Guid.NewGuid());

        await act.Should().ThrowAsync<PropostaNaoEncontradaException>();
    }

    [Fact]
    public async Task ContratarProposta_DeveLancarExcecao_QuandoPropostaNaoAprovada()
    {
        var proposta = new Proposta("Cliente", 100);

        _propostaRepo.Setup(r => r.GetByIdAsync(proposta.Id)).ReturnsAsync(proposta);

        Func<Task> act = async () => await _service.ContratarProposta(proposta.Id);

        await act.Should().ThrowAsync<PropostaNaoAprovadaException>();
    }

    [Fact]
    public async Task ObterPorId_DeveRetornarContratacao()
    {
        var contratacao = new Contratacao(Guid.NewGuid());
        _contratacaoRepo.Setup(r => r.GetByIdAsync(contratacao.Id)).ReturnsAsync(contratacao);

        var result = await _service.ObterPorId(contratacao.Id);

        result.Should().BeEquivalentTo(contratacao);
    }

    [Fact]
    public async Task ObterPorId_DeveLancarExcecao_QuandoNaoEncontrada()
    {
        _contratacaoRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Contratacao?)null);

        Func<Task> act = async () => await _service.ObterPorId(Guid.NewGuid());

        await act.Should().ThrowAsync<ContratacaoNaoEncontradaException>();
    }

    [Fact]
    public async Task ListarContratacoes_DeveRetornarLista()
    {
        var list = new List<Contratacao> { new(Guid.NewGuid()) };
        _contratacaoRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var result = await _service.ListarContratacoes();

        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(list);
    }

    [Fact]
    public async Task ContratarProposta_DeveCriarContratacao_ComValoresCorretos()
    {
        var proposta = new Proposta("Cliente", 100);
        proposta.AlterarStatus(StatusProposta.Aprovada);

        _propostaRepo.Setup(r => r.GetByIdAsync(proposta.Id)).ReturnsAsync(proposta);

        Contratacao? contratacaoCriada = null;
        _contratacaoRepo.Setup(r => r.AddAsync(It.IsAny<Contratacao>()))
                         .Callback<Contratacao>(c => contratacaoCriada = c)
                         .Returns(Task.CompletedTask);

        await _service.ContratarProposta(proposta.Id);

        contratacaoCriada.Should().NotBeNull();
        contratacaoCriada!.PropostaId.Should().Be(proposta.Id);
        contratacaoCriada.DataContratacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task ListarContratacoes_DeveRetornarListaVazia_QuandoNaoExistirem()
    {
        _contratacaoRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Contratacao>());

        var result = await _service.ListarContratacoes();

        result.Should().BeEmpty();
    }
}
