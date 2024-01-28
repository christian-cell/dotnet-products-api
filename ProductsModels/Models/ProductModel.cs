using System.ComponentModel.DataAnnotations;

namespace ProductModels.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Sku { get; set; }
    public string Ean { get; set; }
    public string IVA { get; set; }
    public int Price { get; set; }

    public Product()
    {
        if (Name == null)
        {
            Name = "";
        }
        
        if (Description == null)
        {
            Description = "";
        }
        
        if (Ean == null)
        {
            Ean = "";
        }
        
        if (Name == null)
        {
            Name = "";
        }
    }
}