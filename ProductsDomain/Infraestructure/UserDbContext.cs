using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductsDomain.Entities;

namespace ProductsDomain.Infraestructure;


public class UserDbContext : DbContext
{
    public UserDbContext( DbContextOptions<UserDbContext> options ) : base(options)
    {}
    
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().ToTable("Users", "DotnetProductsPractice");
        base.OnModelCreating(modelBuilder);
    }
}

public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "ProductsAPI"))
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new UserDbContext(optionsBuilder.Options);
    }
}