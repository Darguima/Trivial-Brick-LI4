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

public class BLCatalog(ProductRepository productRepository)
{
  public async Task<Product?> CreateProduct(int model, string name, int price, string description, string image)
  {
    return await productRepository.Add(model, name, price, description, image);
  }

  public async Task<Product?> GetProduct(int model)
  {
    return await productRepository.Find(model);
  }

  public async Task<List<Product>> GetAllProducts()
  {
    return await productRepository.FindAll();
  }
}
