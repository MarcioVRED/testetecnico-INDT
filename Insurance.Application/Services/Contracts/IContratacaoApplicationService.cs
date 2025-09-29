using ContratacaoEntity = Insurance.Domain.Entities.Contratacao;

namespace Insurance.Application.Contracts
{
    public interface IContratacaoApplicationService
    {
        Task ContratarProposta(Guid propostaId);
        Task<IEnumerable<ContratacaoEntity>> ListarContratacoes();
        Task<ContratacaoEntity> ObterPorId(Guid id);
    }
}
