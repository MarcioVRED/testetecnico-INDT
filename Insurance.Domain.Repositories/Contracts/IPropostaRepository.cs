using Insurance.Domain.Entities.Proposta;

namespace Insurance.Domain.Repositories.Contracts;

public interface IPropostaRepository
{
    Task<Proposta?> GetByIdAsync(Guid id);
    Task<IEnumerable<Proposta>> GetAllAsync();
    Task AddAsync(Proposta proposta);
    Task UpdateAsync(Proposta proposta);
}