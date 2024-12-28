namespace TrivialBrick.DataLayer;

public interface IUserRepository
{
  Task<User?> Find(int id);
  Task<List<User>> FindAll();
  Task<User> Update(User card);
  Task Remove(int id);
}