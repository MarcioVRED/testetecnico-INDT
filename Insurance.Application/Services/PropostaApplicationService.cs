using Insurance.Application.Contracts;
using Insurance.Application.Events;
using Insurance.Domain.Entities.Enums;
using Insurance.Domain.Entities.Proposta;
using Insurance.Domain.Repositories.Contracts;
using MassTransit;

public class PropostaApplicationService : IPropostaApplicationService
{
    private readonly IPropostaRepository _repository;
    private readonly IPublishEndpoint _bus;

    public PropostaApplicationService(IPropostaRepository repository, IPublishEndpoint bus)
    {
        _repository = repository;
        _bus = bus;
    }

    public async Task<Proposta> CriarProposta(string cliente, decimal valor)
    {
        var proposta = new Proposta(cliente, valor);
        await _repository.AddAsync(proposta);
        return proposta;
    }

    public async Task<IEnumerable<Proposta>> ListarPropostas() =>
        await _repository.GetAllAsync();

    public async Task<bool> AlterarStatus(Guid id, StatusProposta status)
    {
        var proposta = await _repository.GetByIdAsync(id);
        if (proposta == null) return false;

        proposta.AlterarStatus(status);
        await _repository.UpdateAsync(proposta);

        if (status == StatusProposta.Aprovada)
        {
            await _bus.Publish(new PropostaAprovadaEvent { PropostaId = proposta.Id });
        }

        return true;
    }
}
