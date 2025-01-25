using TrivialBrick.Data.Repositories;
using TrivialBrick.Data.Models;

namespace TrivialBrick.Business;

/*
This business layer is responsible by:

- assembly lines

*/

public class BLAssemblyLines
{
    private readonly AssemblyLineRepository assemblyLineRepository;
    private readonly BLOrders ordersBL;
    private readonly BLCatalog catalogBL;

    public BLAssemblyLines(AssemblyLineRepository assemblyLineRepository, BLOrders ordersBL, BLCatalog catalogBL)
    {
        this.assemblyLineRepository = assemblyLineRepository;
        this.ordersBL = ordersBL;
        this.catalogBL = catalogBL;

        // Call the method to allocate orders to free assembly lines
        _ = TryAllocateOrdersToFreeAssemblyLines();
    }
    
    public async Task<AssemblyLine?> CreateAssemblyLine(string id)
    {
        var line = await assemblyLineRepository.Add(id);

        await TryAllocateOrdersToFreeAssemblyLines();

        return line;
    }

    public async Task<AssemblyLine?> GetAssemblyLine(string id)
    {
        return await assemblyLineRepository.Find(id);
    }

    public async Task<List<AssemblyLine>> GetOccupiedAssemblyLines()
    {
        var list = await assemblyLineRepository.FindAllOccupied();
        return list;
    }

    public async Task<List<AssemblyLine>> GetAllAssemblyLines()
    {
        return await assemblyLineRepository.FindAll();
    }

    public async Task UpdateAssemblyLine(AssemblyLine assemblyLine)
    {
        await assemblyLineRepository.Update(assemblyLine);

        if (assemblyLine.State == AssemblyLineState.Active)
        {
            await TryAllocateOrdersToFreeAssemblyLines();
        }
    }

    public async Task DeleteAssemblyLine(AssemblyLine assemblyLine)
    {
        if (assemblyLine.State == AssemblyLineState.Active)
        {
            throw new Exception("Cannot delete an active assembly line");
        }
        else if (assemblyLine.Order_id != null)
        {
            throw new Exception("Cannot delete an assembly line that is currently processing an order");
        }

        await assemblyLineRepository.Remove(assemblyLine);
    }

    public async Task TryAllocateOrderToAssemblyLine(Order order)
    {
        var freeLines = await assemblyLineRepository.FindAllActiveAndFree();

        if (freeLines != null && freeLines.Count > 0)
        {
            var firstFreeLine = freeLines.First();
            firstFreeLine.Order_id = order.Order_id;
            var date = DateTime.Now;
            firstFreeLine.Mount_start_time = date;
            var amount_of_pieces = await catalogBL.GetProductPartsCount(order.Product_id);
            firstFreeLine.Expected_end_time = date.AddSeconds(amount_of_pieces);
            await assemblyLineRepository.Update(firstFreeLine);
            order.State = OrderState.Assembly_line;
            await ordersBL.UpdateOrder(order);
            await ordersBL.CreateNotification("Your order is now being processed.", DateTime.Now, order.Client_id, order.Order_id);
        }
    }

    public async Task TryAllocateOrdersToFreeAssemblyLines()
    {
        var pendingOrders = await ordersBL.FindAllPendingOrders();

        if (pendingOrders != null)
        {

            foreach (var order in pendingOrders)
            {
                await TryAllocateOrderToAssemblyLine(order);
            }
        }

    }

    public async Task DeallocateAssemblyLine(AssemblyLine assemblyLine)
    {
        if (assemblyLine.Order_id != null)
        {
            var order = await ordersBL.GetOrder(assemblyLine.Order_id.Value);
            if (order != null)
            {
                await ordersBL.CreateNotification("Order is ready", DateTime.Now, order.Client_id, order.Order_id);
                order.State = OrderState.Finished;
                await ordersBL.UpdateOrder(order);
            }
        }

        assemblyLine.Order_id = null;
        assemblyLine.Mount_start_time = null;
        assemblyLine.Expected_end_time = null;
        await assemblyLineRepository.Update(assemblyLine);

        await TryAllocateOrdersToFreeAssemblyLines();
    }
}
