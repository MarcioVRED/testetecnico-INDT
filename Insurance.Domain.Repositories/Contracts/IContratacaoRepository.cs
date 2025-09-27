using Insurance.Domain.Entities.Contratacao;

namespace Insurance.Domain.Repositories.Contracts
{
    public interface IContratacaoRepository
    {
        Task AddAsync(Contratacao contratacao);
        Task<Contratacao?> GetByIdAsync(Guid id);
        Task<IEnumerable<Contratacao>> GetAllAsync();
    }
}
