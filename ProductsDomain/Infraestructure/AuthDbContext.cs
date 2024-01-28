using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductsDomain.Entities;

namespace ProductsDomain.Infraestructure;


public class AuthDbContext : DbContext
{
    public AuthDbContext( DbContextOptions<AuthDbContext> options ) : base(options)
    {}
    
    public DbSet<AuthEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthEntity>().ToTable("Auth", "DotnetProductsPractice");
        base.OnModelCreating(modelBuilder);
    }
}

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "ProductsAPI"))
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new AuthDbContext(optionsBuilder.Options);
    }
}