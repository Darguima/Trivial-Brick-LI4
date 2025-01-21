using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class OrderRepository(ISqlDataAccess db)
    {
        private readonly ISqlDataAccess _db = db;

        public async Task<Order?> Find(int id)
        {
            string sql = "select * from orders where Order_id = @id";
            var orders = await _db.LoadData<Order, dynamic>(sql, new { id });
            return orders?.FirstOrDefault();
        }

        public Task<List<Order>> FindAll()
        {
            string sql = "select * from orders";
            return _db.LoadData<Order, dynamic>(sql, new { });
        }

        public async Task<Order> Add(string address, OrderState state, int product_id, int client_id, decimal price, DateTime date)
        {
            string sql = @"
                insert into orders (Address, State, Product_id, Client_id, Price, Date) 
                values (@address, @state, @product_id, @client_id, @price, @date);
                select SCOPE_IDENTITY();
            ";
            var orderId = await _db.LoadData<int, dynamic>(sql, new { Address = address, State = state.ToString(), Product_id = product_id, Client_id = client_id, Price = price, Date = date });
            int id = orderId.FirstOrDefault();

            Order? order = await Find(id);

            if (order == null)
            {
                throw new Exception("Some unexpected error occurred while creating the order");
            }

            return order;
        }

        public Task Update(Order order)
        {
            string sql = "update orders set Address = @Address, State = @State, Product_id = @Product_id, Client_id = @Client_id, Price = @Price, Date = @Date where Order_id = @Order_id";
            return _db.SaveData(sql, new 
            {
                order.Address,
                State = order.State.ToString(),
                order.Product_id,
                order.Client_id,
                order.Price,
                order.Date,
                order.Order_id
            });
        }

        public Task Remove(Order order)
        {
            string sql = "delete from orders where Order_id = @Order_id";
            return _db.SaveData(sql, order);
        }
    }
}