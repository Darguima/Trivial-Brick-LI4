using TrivialBrick.Data.Repositories;
using TrivialBrick.Data.Models;

namespace TrivialBrick.Business;

/*
This business layer is responsible by:

- orders
- invoices
- notifications

*/

public class BLOrders(OrderRepository orderRepository)
{
    public async Task<Order?> CreateOrder(string address, OrderState state, int product_id, int client_id, decimal price, DateTime date)
    {
        return await orderRepository.Add(address, state, product_id, client_id, price, date);
    }

    public async Task<Order?> GetOrder(int id)
    {
        return await orderRepository.Find(id);
    }

    public async Task<List<Order>> GetAllOrders()
    {
        return await orderRepository.FindAll();
    }

    public async Task UpdateOrder(Order assemblyLine)
    {
        await orderRepository.Update(assemblyLine);
    }

    public async Task DeleteOrder(Order assemblyLine)
    {
        await orderRepository.Remove(assemblyLine);
    }
}