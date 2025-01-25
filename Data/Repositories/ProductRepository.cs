using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class ProductRepository(ISqlDataAccess db)
    {
        private readonly ISqlDataAccess _db = db;

        public async Task<Product?> Find(int model)
        {
            string sql = "select * from products where model = @model";
            var products = await _db.LoadData<Product, dynamic>(sql, new { model });
            return products?.FirstOrDefault();
        }

        public Task<List<Product>> FindAll()
        {
            string sql = "select * from products";
            return _db.LoadData<Product, dynamic>(sql, new { });
        }

        public async Task<Product> Add(int model, string name, decimal price, string description, string image)
        {
            string sql = "insert into products (model, name, price, description, image) values (@Model, @Name, @Price, @Description, @Image)";
            await _db.SaveData(sql, new { Model = model, Name = name, Price = price, Description = description, Image = image });
            return new Product { Model = model, Name = name, Price = price, Description = description, Image = image };
        }

        public Task Update(Product product)
        {
            string sql = "update products set name = @Name, price = @Price, description = @Description, image = @Image where model = @Model";
            return _db.SaveData(sql, product);
        }

        public Task Remove(Product product)
        {
            string sql = "delete from products where model = @Model";
            return _db.SaveData(sql, product);
        }
    }
}