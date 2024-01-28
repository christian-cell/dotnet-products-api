using ProductModels.Models;

namespace ProductsServices.Abstractions;

public interface IProductService
{
    public Task<List<Product>> GetProductsAsync();
}