using TrivialBrick.Data.Repositories;
using TrivialBrick.Data.Models;

namespace TrivialBrick.Business;


/*
This business layer is responsible by:

- users
- admins
- clients

*/

public class BLClients(ClientRepository clientRepository)
{
    public async Task<Client?> AuthenticateUser(string email, string password)
    {
        return await clientRepository.FindByMailPassword(email, password);
    }

    public async Task<Client?> CreateUser(string name, string email, string password, string nif)
    {
        return await clientRepository.Add(name, email, password, nif);
    }
}