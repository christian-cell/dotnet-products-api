using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductModels.Models;
using ProductsServices.Abstractions;

namespace ProductsAPI.Controllers;


[ApiController]
[Route("[controller]")]

public class ProductController : ControllerBase
{

    private IProductService _productService;
    
    public ProductController( IProductService productService )
    {
        _productService = productService;
    }

    // [Authorize]
    [HttpGet("GetProducts")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        return Ok(await _productService.GetProductsAsync());
    }
}