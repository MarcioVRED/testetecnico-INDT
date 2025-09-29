using Insurance.Application.Contracts;
using Insurance.Contratacao.Application.Services.Contracts;
using Insurance.Domain.Entities.Enums;

public class PropostaStatusCheckerService : IPropostaStatusCheckerService
{
    private readonly IPropostaApplicationService _propostaService;

    public PropostaStatusCheckerService(IPropostaApplicationService propostaService)
    {
        _propostaService = propostaService;
    }

    public async Task<bool> EhAprovadaAsync(Guid propostaId)
    {
        var status = await _propostaService.ObterStatusAsync(propostaId);
        return status == StatusProposta.Aprovada;
    }
}
