using TrivialBrick.DataLayer.Models;

namespace TrivialBrick.DataLayer.Repositories;

public interface IClientRepository
{
  Task<Client?> Find(string id);
  Task<List<Client>> FindAll();
  Task<Client> Add(string name, string email, string password, string nif);
  Task Update(Client client);
  Task Remove(string id);

  Task<Client?> Authenticate(string mail, string password);
}