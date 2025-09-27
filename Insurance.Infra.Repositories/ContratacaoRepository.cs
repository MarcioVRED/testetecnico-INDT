using Insurance.Domain.Entities.Contratacao;
using Insurance.Domain.Repositories.Contracts;
using Insurance.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infra.Repositories
{
    public class ContratacaoRepository : IContratacaoRepository
    {
        private readonly InsuranceDbContext _context;

        public ContratacaoRepository(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Contratacao contratacao)
        {
            _context.Contratacoes.Add(contratacao);
            await _context.SaveChangesAsync();
        }

        public async Task<Contratacao?> GetByIdAsync(Guid id)
        {
            return await _context.Contratacoes.FindAsync(id);
        }

        public async Task<IEnumerable<Contratacao>> GetAllAsync()
        {
            return await _context.Contratacoes.ToListAsync();
        }
    }
}
