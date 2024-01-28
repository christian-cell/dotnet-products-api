using ProductModels.Models;

namespace ProductsRepository.Abstractions;

public interface IProductRepository
{
    public Task<List<Product>> GetAllProducts();
}