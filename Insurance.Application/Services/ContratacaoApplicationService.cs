using Insurance.Application.Contracts;
using Insurance.Application.Exceptions;
using Insurance.Domain.Entities.Contratacao;
using Insurance.Domain.Entities.Enums;
using Insurance.Domain.Repositories.Contracts;

namespace Insurance.Application;

public class ContratacaoApplicationService : IContratacaoApplicationService
{
    private readonly IContratacaoRepository _contratacaoRepository;
    private readonly IPropostaRepository _propostaRepository;

    public ContratacaoApplicationService(IContratacaoRepository contratacaoRepository,
                              IPropostaRepository propostaRepository)
    {
        _contratacaoRepository = contratacaoRepository;
        _propostaRepository = propostaRepository;
    }

    public async Task ContratarProposta(Guid propostaId)
    {
        var proposta = await _propostaRepository.GetByIdAsync(propostaId);
        if (proposta == null) throw new PropostaNaoEncontradaException(propostaId);
        if (proposta.Status != StatusProposta.Aprovada) throw new PropostaNaoAprovadaException(propostaId);

        var contratacao = new Contratacao(propostaId);
        await _contratacaoRepository.AddAsync(contratacao);
    }

    public async Task<IEnumerable<Contratacao>> ListarContratacoes()
    {
        return await _contratacaoRepository.GetAllAsync();
    }

    public async Task<Contratacao> ObterPorId(Guid id)
    {
        var contratacao = await _contratacaoRepository.GetByIdAsync(id);
        if (contratacao == null)
            throw new ContratacaoNaoEncontradaException(id);

        return contratacao;
    }
}
