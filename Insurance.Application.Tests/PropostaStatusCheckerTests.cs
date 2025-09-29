using Moq;
using Insurance.Application.Contracts;
using Insurance.Domain.Entities.Enums;
using FluentAssertions;

namespace Insurance.Application.Tests.Services;

public class PropostaStatusCheckerTests
{
    private readonly Mock<IPropostaApplicationService> _propostaService = new();
    private readonly PropostaStatusCheckerService _checker;

    public PropostaStatusCheckerTests()
    {
        _checker = new PropostaStatusCheckerService(_propostaService.Object);
    }

    [Fact]
    public async Task EhAprovadaAsync_DeveRetornarTrue_QuandoPropostaAprovada()
    {
        var propostaId = Guid.NewGuid();
        _propostaService.Setup(p => p.ObterStatusAsync(propostaId))
                        .ReturnsAsync(StatusProposta.Aprovada);

        var result = await _checker.EhAprovadaAsync(propostaId);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task EhAprovadaAsync_DeveRetornarFalse_QuandoPropostaEmAnalise()
    {
        var propostaId = Guid.NewGuid();
        _propostaService.Setup(p => p.ObterStatusAsync(propostaId))
                        .ReturnsAsync(StatusProposta.EmAnalise);

        var result = await _checker.EhAprovadaAsync(propostaId);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task EhAprovadaAsync_DeveRetornarFalse_QuandoPropostaRejeitada()
    {
        var propostaId = Guid.NewGuid();
        _propostaService.Setup(p => p.ObterStatusAsync(propostaId))
                        .ReturnsAsync(StatusProposta.Rejeitada);

        var result = await _checker.EhAprovadaAsync(propostaId);

        result.Should().BeFalse();
    }
}
