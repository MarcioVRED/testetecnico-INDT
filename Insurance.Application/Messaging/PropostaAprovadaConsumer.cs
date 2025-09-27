using Insurance.Application.Commands;
using Insurance.Application.Events;
using MassTransit;
using Insurance.Application.Contracts;

namespace Insurance.Application.Messaging;

public class PropostaAprovadaConsumer : IConsumer<PropostaAprovadaEvent>
{
    private readonly IContratacaoApplicationService _service;

    public PropostaAprovadaConsumer(IContratacaoApplicationService service) => _service = service;

    public async Task Consume(ConsumeContext<PropostaAprovadaEvent> context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (context.Message == null) throw new ArgumentNullException(nameof(context.Message));

        var command = new ContratarPropostaCommand(context.Message.PropostaId);
        await _service.ContratarProposta(command.PropostaId);
    }
}