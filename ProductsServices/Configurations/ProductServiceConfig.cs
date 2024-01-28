namespace ProductsServices.Configurations;

public class ProductServiceConfig
{
    public string DefaultConnection { get; set; }

    public ProductServiceConfig()
    {
        if (DefaultConnection == null)
        {
            DefaultConnection = "";
        }
    }
}