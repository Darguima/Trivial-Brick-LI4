using TrivialBrick.Data.Repositories;
using TrivialBrick.Data.Models;

namespace TrivialBrick.Business;

/*
This business layer is responsible by:

- assembly lines

*/

public class BLAssemblyLines(AssemblyLineRepository assemblyLineRepository)
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
}
