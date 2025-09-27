using Insurance.Domain.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Insurance.Domain.Entities.Proposta;
using Insurance.Infra.Persistence;

namespace Insurance.Infra.Repositories;

public class PropostaRepository : IPropostaRepository
{
    private readonly InsuranceDbContext _context;

    public PropostaRepository(InsuranceDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Proposta proposta)
    {
        _context.Propostas.Add(proposta);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Proposta>> GetAllAsync()
    {
        return await _context.Propostas.ToListAsync();
    }

    public async Task<Proposta?> GetByIdAsync(Guid id)
    {
        return await _context.Propostas.FindAsync(id);
    }

    public async Task UpdateAsync(Proposta proposta)
    {
        _context.Propostas.Update(proposta);
        await _context.SaveChangesAsync();
    }
}
