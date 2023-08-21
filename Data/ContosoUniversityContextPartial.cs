using Microsoft.EntityFrameworkCore;

namespace EFCore7UnableDoMigration.Domain.Data;

public partial class ContosoUniversityContext : DbContext
{
    public ContosoUniversityContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ContosoUniversity;Integrated Security=True");
    }
}