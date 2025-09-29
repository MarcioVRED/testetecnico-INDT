using Insurance.Application.Contracts;
using Insurance.Application.Events;
using Insurance.Application.Exceptions;
using Insurance.Domain.Entities.Enums;
using Insurance.Domain.Entities.Proposta;
using Insurance.Domain.Repositories.Contracts;
using Insurance.Infra.Repositories;
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

    public async Task<bool> AlterarStatus(Guid id, StatusProposta novoStatus)
    {
        var proposta = await _repository.GetByIdAsync(id);
        if (proposta == null)
        {
            throw new PropostaNaoEncontradaException(id);
        }

        if (proposta.Status == StatusProposta.Rejeitada && novoStatus == StatusProposta.Aprovada)
        {
            throw new AlteracaoStatusPropostaInvalidaException(id);
        }

        proposta.AlterarStatus(novoStatus);
        await _repository.UpdateAsync(proposta);

        if (novoStatus == StatusProposta.Aprovada)
        {
            await _bus.Publish(new PropostaAprovadaEvent { PropostaId = proposta.Id });
        }

        return true;
    }

    public async Task<StatusProposta> ObterStatusAsync(Guid propostaId)
    {
        var proposta = await _repository.GetByIdAsync(propostaId);
        if (proposta == null)
            throw new PropostaNaoEncontradaException(propostaId);

        return proposta.Status;
    }
}
