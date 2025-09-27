using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Insurance.ContratacaoService.Infra.Persistence;

public class ContratacaoDbContextFactory : IDesignTimeDbContextFactory<ContratacaoDbContext>
{
    public ContratacaoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ContratacaoDbContext>();

        optionsBuilder.UseSqlServer("Server=localhost,1434;Database=ContratacaoDb;User Id=sa;Password=SenhaForte12345;TrustServerCertificate=True;");

        return new ContratacaoDbContext(optionsBuilder.Options);
    }
}