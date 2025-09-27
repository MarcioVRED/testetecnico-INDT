using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Insurance.PropostaService.Infra.Persistence;

public class PropostaDbContextFactory : IDesignTimeDbContextFactory<PropostaDbContext>
{
    public PropostaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PropostaDbContext>();

        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=PropostaDb;User Id=sa;Password=SenhaForte12345;TrustServerCertificate=True;");

        return new PropostaDbContext(optionsBuilder.Options);
    }
}