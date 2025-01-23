using TrivialBrick.Data.Repositories;
using TrivialBrick.Data.Models;
using System.Threading.Tasks;

namespace TrivialBrick.Business;

/*
This business layer is responsible by:

- assembly lines

*/

public class BLAssemblyLines(AssemblyLineRepository assemblyLineRepository, BLOrders ordersBL)
{
    public async Task<AssemblyLine?> CreateAssemblyLine(string id)
    {
        return await assemblyLineRepository.Add(id);
    }

    public async Task<AssemblyLine?> GetAssemblyLine(string id)
    {
        return await assemblyLineRepository.Find(id);
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
            await TryAlocateOrdersToFreeAssemblyLines();
        }
    }

    public async Task DeleteAssemblyLine(AssemblyLine assemblyLine)
    {
        await assemblyLineRepository.Remove(assemblyLine);
    }

    public async Task TryAllocateOrderToAssemblyLine(Order order) 
    {
        var freeLines = await assemblyLineRepository.FindAllActiveAndFree();

        if (freeLines!=null && freeLines.Count > 0) {
            var firstFreeLine = freeLines.First();
            firstFreeLine.Order_id = order.Order_id;
            await assemblyLineRepository.Update(firstFreeLine);
            order.State = OrderState.Assembly_line;
            await ordersBL.UpdateOrder(order);
            await ordersBL.CreateNotification("Order queued for assembly line", DateTime.Now, order.Client_id, order.Order_id);
        }
            
        

    }

    public async Task TryAlocateOrdersToFreeAssemblyLines()
    {
       var pendingOrders = await ordersBL.FindAllPendingOrders();

        if (pendingOrders!= null) {
       
           foreach (var order in pendingOrders)
           {
               await TryAllocateOrderToAssemblyLine(order);
           }

        }
       
    }

}
