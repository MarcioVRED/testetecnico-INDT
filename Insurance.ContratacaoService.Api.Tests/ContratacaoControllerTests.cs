using Insurance.Application.Contracts;
using Insurance.Application.Dtos;
using Insurance.Application.Exceptions;
using Insurance.ContratacaoService.Api.Controllers;
using ContratacaoEntity = Insurance.Domain.Entities.Contratacao;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Insurance.ContratacaoService.Api.Tests.Controllers;

public class ContratacaoControllerTests
{
    private readonly Mock<IContratacaoApplicationService> _serviceMock;
    private readonly ContratacaoController _controller;

    public ContratacaoControllerTests()
    {
        _serviceMock = new Mock<IContratacaoApplicationService>();
        _controller = new ContratacaoController(_serviceMock.Object);
    }

    [Fact]
    public async Task Contratar_DeveRetornarOk_QuandoSucesso()
    {

        var propostaId = Guid.NewGuid();
        _serviceMock.Setup(s => s.ContratarProposta(propostaId))
                    .Returns(Task.CompletedTask);

        var result = await _controller.Contratar(propostaId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Proposta contratada com sucesso!", okResult.Value);
    }

    [Fact]
    public async Task GetAll_DeveRetornarListaDeContratacoes()
    {
        var contratacao1 = CriarContratacao(Guid.NewGuid(), Guid.NewGuid());
        var contratacao2 = CriarContratacao(Guid.NewGuid(), Guid.NewGuid());

        _serviceMock.Setup(s => s.ListarContratacoes())
                    .ReturnsAsync(new List<ContratacaoEntity> { contratacao1, contratacao2 });

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var dtos = Assert.IsAssignableFrom<IEnumerable<ContratacaoDto>>(okResult.Value);
        Assert.Equal(2, dtos.Count());
    }

    [Fact]
    public async Task GetById_DeveRetornarOk_QuandoEncontrado()
    {
        var id = Guid.NewGuid();
        var contratacao = CriarContratacao(id, Guid.NewGuid());

        _serviceMock.Setup(s => s.ObterPorId(id))
                    .ReturnsAsync(contratacao);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<ContratacaoDto>(okResult.Value);
        Assert.Equal(id, dto.Id);
    }

    [Fact]
    public async Task GetById_DeveLancarExcecao_QuandoNaoEncontrado()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.ObterPorId(id))
                    .ThrowsAsync(new ContratacaoNaoEncontradaException(id));

        await Assert.ThrowsAsync<ContratacaoNaoEncontradaException>(() => _controller.GetById(id));
    }

    [Fact]
    public async Task Contratar_DeveLancarExcecao_QuandoPropostaNaoEncontrada()
    {
        var propostaId = Guid.NewGuid();
        _serviceMock.Setup(s => s.ContratarProposta(propostaId))
                    .ThrowsAsync(new PropostaNaoEncontradaException(propostaId));

        await Assert.ThrowsAsync<PropostaNaoEncontradaException>(() => _controller.Contratar(propostaId));
    }

    private ContratacaoEntity CriarContratacao(Guid id, Guid propostaId)
    {
        var contratacao = (ContratacaoEntity)Activator.CreateInstance(
            typeof(ContratacaoEntity),
            nonPublic: true)!;

        typeof(ContratacaoEntity).GetProperty("Id")!
            .SetValue(contratacao, id);

        typeof(ContratacaoEntity).GetProperty("PropostaId")!
            .SetValue(contratacao, propostaId);

        typeof(ContratacaoEntity).GetProperty("DataContratacao")!
            .SetValue(contratacao, DateTime.UtcNow);

        return contratacao;
    }
}
