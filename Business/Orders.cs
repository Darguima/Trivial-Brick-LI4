using TrivialBrick.Data.Repositories;
using TrivialBrick.Data.Models;

namespace TrivialBrick.Business;

/*
This business layer is responsible by:

- orders
- invoices
- notifications

*/

public class BLOrders(OrderRepository orderRepository, NotificationRepository notificationRepository, InvoiceRepository invoiceRepository)
{
    public async Task<Order?> CreateOrder(string address, OrderState state, int product_id, int client_id, decimal price, DateTime date)
    {
        var order = await orderRepository.Add(address, state, product_id, client_id, price, date);

        if (order != null)
        {
            await CreateInvoice(DateTime.Now, client_id, order.Order_id);
            await CreateNotification("Order queued for assembly line", date, client_id, order.Order_id);

            // await  assemblyLinesBL.TryAllocateOrderToAssemblyLine(order);
        }

        return order;
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

    public async Task<Invoice?> CreateInvoice(DateTime datetime, int client_id, int order_id)
    {
        return await invoiceRepository.Add(datetime, client_id, order_id);
    }

    public async Task<Invoice?> GetInvoice(int id)
    {
        return await invoiceRepository.Find(id);
    }

    public async Task<List<Invoice>> GetAllInvoices()
    {
        return await invoiceRepository.FindAll();
    }

    public async Task UpdateInvoice(Invoice invoice)
    {
        await invoiceRepository.Update(invoice);
    }

    public async Task DeleteInvoice(Invoice invoice)
    {
        await invoiceRepository.Remove(invoice);
    }

    public async Task<Notification?> CreateNotification(string message, DateTime datetime, int client_id, int order_id)
    {
        return await notificationRepository.Add(message, datetime, client_id, order_id);
    }

    public async Task<Notification?> GetNotification(int id)
    {
        return await notificationRepository.Find(id);
    }

    public async Task<List<Notification>> GetAllNotifications()
    {
        return await notificationRepository.FindAll();
    }

    public async Task UpdateNotification(Notification notification)
    {
        await notificationRepository.Update(notification);
    }

    public async Task DeleteNotification(Notification notification)
    {
        await notificationRepository.Remove(notification);
    }

    public async Task<List<Order>> FindAllPendingOrders()
    {
        return await orderRepository.FindAllPendingOrders();
    }
}