using FluentAssertions;
using Insurance.Application.Contracts;
using Insurance.Application.Events;
using Insurance.Application.Messaging;
using MassTransit;
using Moq;

namespace Insurance.Application.Tests.Messaging;

public class PropostaAprovadaConsumerTests
{
    private readonly Mock<IContratacaoApplicationService> _serviceMock = new();
    private readonly PropostaAprovadaConsumer _consumer;

    public PropostaAprovadaConsumerTests()
    {
        _consumer = new PropostaAprovadaConsumer(_serviceMock.Object);
    }

    [Fact]
    public async Task Consume_DeveChamarContratarProposta()
    {
        var propostaId = Guid.NewGuid();
        var contextMock = new Mock<ConsumeContext<PropostaAprovadaEvent>>();
        contextMock.SetupGet(c => c.Message).Returns(new PropostaAprovadaEvent { PropostaId = propostaId });

        await _consumer.Consume(contextMock.Object);

        _serviceMock.Verify(s => s.ContratarProposta(propostaId), Times.Once);
    }

    [Fact]
    public async Task Consume_DeveChamarContratarProposta_CaminhoFeliz()
    {
        var propostaId = Guid.NewGuid();
        var contextMock = new Mock<ConsumeContext<PropostaAprovadaEvent>>();
        contextMock.SetupGet(c => c.Message)
                   .Returns(new PropostaAprovadaEvent { PropostaId = propostaId });

        await _consumer.Consume(contextMock.Object);

        _serviceMock.Verify(s => s.ContratarProposta(propostaId), Times.Once);
    }

    [Fact]
    public async Task Consume_DeveChamarContratarProposta_MesmoComIdVazio()
    {
        var propostaId = Guid.Empty;
        var contextMock = new Mock<ConsumeContext<PropostaAprovadaEvent>>();
        contextMock.SetupGet(c => c.Message)
                   .Returns(new PropostaAprovadaEvent { PropostaId = propostaId });

        await _consumer.Consume(contextMock.Object);

        _serviceMock.Verify(s => s.ContratarProposta(propostaId), Times.Once);
    }

    [Fact]
    public async Task Consume_PropagaExcecao_DoService()
    {
        var propostaId = Guid.NewGuid();
        var contextMock = new Mock<ConsumeContext<PropostaAprovadaEvent>>();
        contextMock.SetupGet(c => c.Message)
                   .Returns(new PropostaAprovadaEvent { PropostaId = propostaId });

        _serviceMock.Setup(s => s.ContratarProposta(propostaId))
                    .ThrowsAsync(new InvalidOperationException("Erro teste"));

        Func<Task> act = async () => await _consumer.Consume(contextMock.Object);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Erro teste");

        _serviceMock.Verify(s => s.ContratarProposta(propostaId), Times.Once);
    }

    [Fact]
    public async Task Consume_DeveLancarArgumentNullException_SeContextNulo()
    {
        Func<Task> act = async () => await _consumer.Consume(null!);

        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("*context*");
    }


    [Fact]
    public async Task Consume_ThrowsArgumentNullException_SeMessageNulo()
    {
        var contextMock = new Mock<ConsumeContext<PropostaAprovadaEvent>>();
        contextMock.SetupGet(c => c.Message).Returns((PropostaAprovadaEvent)default!);

        Func<Task> act = async () => await _consumer.Consume(contextMock.Object);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
