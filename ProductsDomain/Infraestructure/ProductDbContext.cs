using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductsDomain.Entities;

namespace ProductsDomain.Infraestructure;

public class ProductDbContext : DbContext
{
    public ProductDbContext( DbContextOptions<ProductDbContext> options ) : base(options)
    {}
    
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>().ToTable("Products", "ProductsSchema");
        base.OnModelCreating(modelBuilder);
    }
}

public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "ProductsAPI"))
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new ProductDbContext(optionsBuilder.Options);
    }
}