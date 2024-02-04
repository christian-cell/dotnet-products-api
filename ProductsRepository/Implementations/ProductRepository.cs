using Microsoft.EntityFrameworkCore;
using ProductModels.Models;
using ProductsRepository.Abstractions;

namespace ProductsRepository.Implemetations;

public class ProductDbContext : DbContext
{
    public ProductDbContext( DbContextOptions<ProductDbContext> options ) : base(options)
    {}
    
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Products", "ProductsSchema");
        base.OnModelCreating(modelBuilder);
    }
}


public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _dbContext;

    public ProductRepository(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await _dbContext.Products.ToListAsync();
    }
}