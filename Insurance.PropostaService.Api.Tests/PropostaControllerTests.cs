using Insurance.Application.Contracts;
using Insurance.Domain.Entities.Enums;
using Insurance.Domain.Entities.Proposta;
using Insurance.PropostaService.Api.Controllers;
using Insurance.PropostaService.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Insurance.PropostaService.Api.Tests.Controllers;

public class PropostaControllerTests
{
    private readonly Mock<IPropostaApplicationService> _serviceMock;
    private readonly PropostaController _controller;

    public PropostaControllerTests()
    {
        _serviceMock = new Mock<IPropostaApplicationService>();
        _controller = new PropostaController(_serviceMock.Object);
    }

    [Fact]
    public async Task Criar_DeveRetornarOk_QuandoValido()
    {
        var dto = new CreatePropostaDto { Cliente = "João", Valor = 1000m };
        var proposta = new Proposta(dto.Cliente, dto.Valor);

        _serviceMock.Setup(s => s.CriarProposta(dto.Cliente, dto.Valor))
                    .ReturnsAsync(proposta);

        var result = await _controller.Criar(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseDto = Assert.IsType<PropostaDto>(okResult.Value);
        Assert.Equal(proposta.Id, responseDto.Id);
        Assert.Equal(proposta.Cliente, responseDto.Cliente);
    }

    [Fact]
    public async Task Criar_DeveRetornarBadRequest_QuandoDtoInvalido()
    {
        _controller.ModelState.AddModelError("Cliente", "Obrigatório");
        var dto = new CreatePropostaDto { Cliente = "", Valor = 0m };

        var result = await _controller.Criar(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Listar_DeveRetornarOk_ComListaDePropostas()
    {
        var propostas = new List<Proposta>
        {
            new Proposta("Cliente1", 100),
            new Proposta("Cliente2", 200)
        };

        _serviceMock.Setup(s => s.ListarPropostas())
                    .ReturnsAsync(propostas);

        var result = await _controller.Listar();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var dtos = Assert.IsAssignableFrom<IEnumerable<PropostaDto>>(okResult.Value);
        Assert.Equal(2, dtos.Count());
    }

    [Fact]
    public async Task AlterarStatus_DeveRetornarOk_QuandoSucesso()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateStatusPropostaDto { Status = StatusProposta.Aprovada };

        _serviceMock.Setup(s => s.AlterarStatus(id, dto.Status))
                    .ReturnsAsync(true);

        var result = await _controller.AlterarStatus(id, dto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AlterarStatus_DeveRetornarNotFound_QuandoNaoEncontrado()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateStatusPropostaDto { Status = StatusProposta.Aprovada };

        _serviceMock.Setup(s => s.AlterarStatus(id, dto.Status))
                    .ReturnsAsync(false);

        var result = await _controller.AlterarStatus(id, dto);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains(id.ToString(), notFound.Value?.ToString());
    }

    [Fact]
    public async Task AlterarStatus_DeveRetornarBadRequest_QuandoModelStateInvalido()
    {
        _controller.ModelState.AddModelError("Status", "Status inválido");
        var id = Guid.NewGuid();
        var dto = new UpdateStatusPropostaDto { Status = StatusProposta.Aprovada };

        var result = await _controller.AlterarStatus(id, dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);

        var errors = Assert.IsType<SerializableError>(badRequest.Value);
        Assert.True(errors.ContainsKey("Status"));
    }

    [Fact]
    public async Task Criar_DevePropagarExcecao_DoService()
    {
        var dto = new CreatePropostaDto { Cliente = "João", Valor = 1000m };

        _serviceMock.Setup(s => s.CriarProposta(dto.Cliente, dto.Valor))
                    .ThrowsAsync(new Exception("Erro inesperado"));

        var ex = await Assert.ThrowsAsync<Exception>(() => _controller.Criar(dto));

        Assert.Equal("Erro inesperado", ex.Message);
    }
}
