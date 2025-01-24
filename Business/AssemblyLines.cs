using TrivialBrick.Data.Repositories;
using TrivialBrick.Data.Models;
using System.Threading.Tasks;

namespace TrivialBrick.Business;

/*
This business layer is responsible by:

- assembly lines

*/

public class BLAssemblyLines(AssemblyLineRepository assemblyLineRepository, BLOrders ordersBL, BLCatalog catalogBL)
{
    public async Task<AssemblyLine?> CreateAssemblyLine(string id)
    {
        return await assemblyLineRepository.Add(id);
    }

    public async Task<AssemblyLine?> GetAssemblyLine(string id)
    {
        return await assemblyLineRepository.Find(id);
    }

      public async Task<List<AssemblyLine>> GetOcupiedAssemblyLines()
    {
        var list = await assemblyLineRepository.FindAllOcupied();
        return list;
    }

    public async Task<List<AssemblyLine>> GetAllAssemblyLines()
    {
        return await assemblyLineRepository.FindAll();
    }

    public async Task UpdateAssemblyLine(AssemblyLine assemblyLine)
    {
        // if new state is inactive and there is a product, dont update 
        if (assemblyLine.State == AssemblyLineState.Inactive && assemblyLine.Order_id != null)
        {
            return;
        }

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
            var date = DateTime.Now;
            firstFreeLine.Mount_start_time= date;
            var amount_of_pieces = await catalogBL.GetProductPartsCount(order.Product_id);
            firstFreeLine.Expected_end_time = date.AddSeconds(amount_of_pieces);
            await assemblyLineRepository.Update(firstFreeLine);
            order.State = OrderState.Assembly_line;
            await ordersBL.UpdateOrder(order);
            await ordersBL.CreateNotification("Your order is now being processed.", DateTime.Now, order.Client_id, order.Order_id);
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

     public async Task DesalocateAssemblyLine(AssemblyLine assemblyLine)
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
    }

}
