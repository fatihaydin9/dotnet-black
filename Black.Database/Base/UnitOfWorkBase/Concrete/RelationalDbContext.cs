using Microsoft.EntityFrameworkCore;

namespace Black.Infrastructure.UnitOfWorkBase.Concrete;
public class RelationalDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Server=127.0.0.1;Port=5432;Database=userdb;UserId=postgres;Password=2149357;
");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}