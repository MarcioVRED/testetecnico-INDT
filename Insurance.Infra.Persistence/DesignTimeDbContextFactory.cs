using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Insurance.Infra.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<InsuranceDbContext>
    {
        public InsuranceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<InsuranceDbContext>();

            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=InsuranceDb;User Id=sa;Password=Passw0rd!;",
                sql => sql.MigrationsAssembly("Insurance.Infra.Persistence"));

            return new InsuranceDbContext(optionsBuilder.Options);
        }
    }
}
