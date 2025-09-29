using Insurance.Application.Contracts;
using Insurance.Contratacao.Application.Services.Contracts;
using Insurance.Domain.Entities.Enums;

public class PropostaStatusChecker : IPropostaStatusChecker
{
    private readonly IPropostaApplicationService _propostaService;

    public PropostaStatusChecker(IPropostaApplicationService propostaService)
    {
        _propostaService = propostaService;
    }

    public async Task<bool> EhAprovadaAsync(Guid propostaId)
    {
        var status = await _propostaService.ObterStatusAsync(propostaId);
        return status == StatusProposta.Aprovada;
    }
}
