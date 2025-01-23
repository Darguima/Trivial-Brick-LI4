using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class ProductPartRepository
    {
        private readonly ISqlDataAccess _db;

        public ProductPartRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<ProductPart?> FindProductPart(int partId, int productId)
        {
            string sql = "select * from products_parts where part_id = @partId and product_id = @productId";
            var productParts = await _db.LoadData<ProductPart, dynamic>(sql, new { partId, productId });
            return productParts?.FirstOrDefault();
        }

        public Task<List<ProductPart>> FindAllProductParts()
        {
            string sql = "select * from products_parts";
            return _db.LoadData<ProductPart, dynamic>(sql, new { });
        }

        public async Task<ProductPart> AddProductPart(int partId, int productId, int quantity)
        {
            string sql = @"
                insert into products_parts (part_id, product_id, quantity) 
                values (@partId, @productId, @quantity);
            ";
            await _db.SaveData(sql, new { partId, productId, quantity });

            ProductPart? productPart = await FindProductPart(partId, productId);

            if (productPart == null)
            {
                throw new Exception("Some unexpected error occurred while creating the product part");
            }

            return productPart;
        }

        public Task UpdateProductPart(ProductPart productPart)
        {
            string sql = "update products_parts set quantity = @Quantity where part_id = @Part_id and product_id = @Product_id";
            return _db.SaveData(sql, productPart);
        }

        public Task RemoveProductPart(ProductPart productPart)
        {
            string sql = "delete from products_parts where part_id = @Part_id and product_id = @Product_id";
            return _db.SaveData(sql, productPart);
        }
    }
}