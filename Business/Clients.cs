using TrivialBrick.Data.Repositories;
using TrivialBrick.Data.Models;

namespace TrivialBrick.Business;


/*
This business layer is responsible by:

- users
- admins
- clients

*/

public class BLClients(ClientRepository clientRepository, AdminRepository adminRepository)
{
    public async Task<Client?> AuthenticateUser(string email, string password)
    {
        return await clientRepository.FindByMailPassword(email, password);
    }

    public async Task<Client?> CreateUser(string name, string email, string password, string nif)
    {
        return await clientRepository.Add(name, email, password, nif);
    }

    public async Task<Admin?> AuthenticateAdmin(string email, string password)
    {
        return await adminRepository.FindByMailPassword(email, password);
    }

    public async Task<string> GetClientNif(int id)
    {
        // returning the nif of a client by its id
        return (await clientRepository.GetNIF(id)).ToString();
    }
}