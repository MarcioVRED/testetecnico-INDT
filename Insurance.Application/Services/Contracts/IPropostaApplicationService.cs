
using Insurance.Domain.Entities.Enums;
using Insurance.Domain.Entities.Proposta;

namespace Insurance.Application.Contracts
{
    public interface IPropostaApplicationService
    {
        Task<Proposta> CriarProposta(string cliente, decimal valor);
        Task<IEnumerable<Proposta>> ListarPropostas();
        Task<bool> AlterarStatus(Guid id, StatusProposta status);
        Task<StatusProposta> ObterStatusAsync(Guid propostaId);
    }
}
