using Insurance.Application.Contracts;
using Insurance.Application.Exceptions;
using Insurance.Contratacao.Application.Services.Contracts;
using Insurance.Domain.Repositories.Contracts;
using ContratacaoEntity = Insurance.Domain.Entities.Contratacao;

namespace Insurance.Application;

public class ContratacaoApplicationService : IContratacaoApplicationService
{
    private readonly IContratacaoRepository _contratacaoRepository;
    private readonly IPropostaStatusCheckerService _propostaStatusChecker;

    public ContratacaoApplicationService(IContratacaoRepository contratacaoRepository, IPropostaStatusCheckerService propostaStatusChecker)
    {
        _contratacaoRepository = contratacaoRepository;
        _propostaStatusChecker = propostaStatusChecker;
    }

    public async Task ContratarProposta(Guid propostaId)
    {
        var estaAprovada = await _propostaStatusChecker.EhAprovadaAsync(propostaId);
        if (!estaAprovada)
            throw new PropostaNaoAprovadaException(propostaId);

        var contratacao = new ContratacaoEntity(propostaId);
        await _contratacaoRepository.AddAsync(contratacao);
    }

    public async Task<IEnumerable<ContratacaoEntity>> ListarContratacoes()
    {
        return await _contratacaoRepository.GetAllAsync();
    }

    public async Task<ContratacaoEntity> ObterPorId(Guid id)
    {
        var contratacao = await _contratacaoRepository.GetByIdAsync(id);
        if (contratacao == null)
            throw new ContratacaoNaoEncontradaException(id);

        return contratacao;
    }
}
