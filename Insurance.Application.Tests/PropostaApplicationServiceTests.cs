using FluentAssertions;
using Insurance.Application.Events;
using Insurance.Application.Exceptions;
using Insurance.Domain.Entities.Enums;
using Insurance.Domain.Entities.Proposta;
using Insurance.Domain.Repositories.Contracts;
using MassTransit;
using Moq;

namespace Insurance.Application.Tests.Services;

public class PropostaApplicationServiceTests
{
    private readonly Mock<IPropostaRepository> _repoMock = new();
    private readonly Mock<IPublishEndpoint> _busMock = new();
    private readonly PropostaApplicationService _service;

    public PropostaApplicationServiceTests()
    {
        _service = new PropostaApplicationService(_repoMock.Object, _busMock.Object);
    }

    [Fact]
    public async Task CriarProposta_DeveAdicionarProposta()
    {
        var cliente = "Cliente Teste";
        var valor = 100m;

        var proposta = await _service.CriarProposta(cliente, valor);

        proposta.Cliente.Should().Be(cliente);
        proposta.Valor.Should().Be(valor);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Proposta>()), Times.Once);
    }

    [Fact]
    public async Task ListarPropostas_DeveRetornarLista()
    {
        var list = new List<Proposta> { new("Cliente 1", 100) };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var result = await _service.ListarPropostas();

        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(list);
    }

    [Fact]
    public async Task AlterarStatus_DeveAlterarStatusEPublicarEvent()
    {
        var proposta = new Proposta("Cliente", 100);
        _repoMock.Setup(r => r.GetByIdAsync(proposta.Id)).ReturnsAsync(proposta);

        var result = await _service.AlterarStatus(proposta.Id, StatusProposta.Aprovada);

        result.Should().BeTrue();
        proposta.Status.Should().Be(StatusProposta.Aprovada);
        _repoMock.Verify(r => r.UpdateAsync(proposta), Times.Once);
        _busMock.Verify(b => b.Publish(It.IsAny<PropostaAprovadaEvent>(), default), Times.Once);
    }

    [Fact]
    public async Task AlterarStatus_DeveAtualizarStatusSemPublicarEvent_QuandoNaoAprovada()
    {
        var proposta = new Proposta("Cliente", 100);
        _repoMock.Setup(r => r.GetByIdAsync(proposta.Id)).ReturnsAsync(proposta);

        var result = await _service.AlterarStatus(proposta.Id, StatusProposta.Rejeitada);

        result.Should().BeTrue();
        proposta.Status.Should().Be(StatusProposta.Rejeitada);
        _repoMock.Verify(r => r.UpdateAsync(proposta), Times.Once);
        _busMock.Verify(b => b.Publish(It.IsAny<PropostaAprovadaEvent>(), default), Times.Never);
    }

    [Fact]
    public async Task AlterarStatus_DeveLancarExcecao_QuandoPropostaNaoEncontrada()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Proposta?)null);

        Func<Task> act = async () => await _service.AlterarStatus(Guid.NewGuid(), StatusProposta.Aprovada);

        await act.Should().ThrowAsync<PropostaNaoEncontradaException>();
    }

    [Fact]
    public async Task AlterarStatus_DeveLancarExcecao_AoTentarAprovarPropostaRejeitada()
    {
        var proposta = new Proposta("Cliente", 100);
        proposta.AlterarStatus(StatusProposta.Rejeitada); // Status inicial é Rejeitada
        _repoMock.Setup(r => r.GetByIdAsync(proposta.Id)).ReturnsAsync(proposta);

        Func<Task> act = async () => await _service.AlterarStatus(proposta.Id, StatusProposta.Aprovada);

        await act.Should().ThrowAsync<AlteracaoStatusPropostaInvalidaException>();
    }
}
