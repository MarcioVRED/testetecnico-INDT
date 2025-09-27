using Insurance.Domain.Entities.Contratacao;

namespace Insurance.Application.Contracts
{
    public interface IContratacaoApplicationService
    {
        Task ContratarProposta(Guid propostaId);
        Task<IEnumerable<Contratacao>> ListarContratacoes();
        Task<Contratacao> ObterPorId(Guid id);
    }
}
