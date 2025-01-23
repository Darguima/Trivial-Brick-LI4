using TrivialBrick.Data.Repositories;
using TrivialBrick.Data.Models;

namespace TrivialBrick.Business;

/*
This business layer is responsible by:

- products
- parts
- products & parts relation
- instructions

*/

public class BLCatalog(ProductRepository productRepository, PartRepository partRepository, ProductPartRepository productPartRepository)
{
    public async Task<Product?> CreateProduct(int model, string name, int price, string description, string image, List<Tuple<int, int>>? parts_list = null)
    {
        var product = await productRepository.Add(model, name, price, description, image);

        if (product != null && parts_list != null)
        {
            foreach (var part in parts_list)
            {
                await AddPartToProduct(model, part.Item1, part.Item2);
            }
        }

        return product;
    }

    public async Task AddPartToProduct(int productId, int partId, int quantity)
    {
        await productPartRepository.AddProductPart(partId, productId, quantity);
    }

    public async Task<Product?> GetProduct(int model)
    {
        return await productRepository.Find(model);
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await productRepository.FindAll();
    }

    public async Task UpdateProduct(Product product)
    {
        await productRepository.Update(product);
    }

    public async Task RemoveProduct(Product product)
    {
        await productRepository.Remove(product);
    }

    public async Task<Part?> CreatePart(int partId, string? image, int stock)
    {
        return await partRepository.AddPart(partId, image, stock);
    }

    public async Task<Part?> GetPart(int partId)
    {
        return await partRepository.FindPart(partId);
    }

    public async Task<List<Part>> GetAllParts()
    {
        return await partRepository.FindAllParts();
    }

    public async Task UpdatePart(Part part)
    {
        await partRepository.UpdatePart(part);
    }

    public async Task RemovePart(Part part)
    {
        await partRepository.RemovePart(part);
    }

    public async Task<ProductPart?> GetProductPart(int partId, int productId)
    {
        return await productPartRepository.FindProductPart(partId, productId);
    }

    public async Task<List<ProductPart>> GetAllProductParts()
    {
        return await productPartRepository.FindAllProductParts();
    }

    public async Task UpdateProductPart(ProductPart productPart)
    {
        await productPartRepository.UpdateProductPart(productPart);
    }

    public async Task RemoveProductPart(ProductPart productPart)
    {
        await productPartRepository.RemoveProductPart(productPart);
    }


}
