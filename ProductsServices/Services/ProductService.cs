using ProductModels.Models;
using ProductsRepository.Abstractions;
using ProductsServices.Abstractions;

namespace ProductsServices.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    
    public ProductService( 
        IProductRepository productRepository
    )
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _productRepository.GetAllProducts();
    }
}