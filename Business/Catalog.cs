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

public class BLCatalog(ProductRepository productRepository, PartRepository partRepository, ProductPartRepository productPartRepository, InstructionRepository instructionRepository)
{
    public async Task<Product?> CreateProduct(int model, string name, decimal price, string description, string image, List<Tuple<int, int>>? parts_list = null, List<Tuple<string, int>>? instructions = null)
    {
        parts_list ??= [];
        instructions ??= [];

        var product = await productRepository.Add(model, name, price, description, image);

        if (product != null)
        {
            foreach (var part in parts_list)
            {
                await AddPartToProduct(model, part.Item1, part.Item2);
            }

            for (int i = 0; i < instructions.Count; i++)
            {
                var instruction = instructions[i];
                await AddInstructionToProduct(model, i, instruction.Item1, instruction.Item2);
            }
        }

        return product;
    }

    public async Task AddPartToProduct(int productId, int partId, int quantity)
    {
        await productPartRepository.AddProductPart(partId, productId, quantity);
    }

    public async Task AddInstructionToProduct(int productId, int seqNum, string image, int qntParts)
    {
        await instructionRepository.AddInstruction(productId, seqNum, image, qntParts);
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

    public async Task<Part?> CreatePart(int partId, string image, int stock)
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

    public async Task UpdateProductPart(ProductPart productPart)
    {
        await productPartRepository.UpdateProductPart(productPart);
    }

    public async Task RemoveProductPart(ProductPart productPart)
    {
        await productPartRepository.RemoveProductPart(productPart);
    }

    public async Task<Instruction?> GetInstruction(int productId, int seqNum)
    {
        return await instructionRepository.FindInstruction(productId, seqNum);
    }

    public async Task UpdateInstruction(Instruction instruction)
    {
        await instructionRepository.UpdateInstruction(instruction);
    }

    public async Task RemoveInstruction(Instruction instruction)
    {
        await instructionRepository.RemoveInstruction(instruction);
    }
    public async Task<int> GetProductPartsCount(int productId)
    {
        var productParts = await productPartRepository.FindAllProductPartsByProduct(productId);
        if (productParts != null)
        {
            return productParts.Count;
        }
        else
        {
            throw new Exception("No parts found for this product");
        }
    }
}
