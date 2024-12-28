namespace TrivialBrick.DataLayer;

public interface IUserRepository
{
  Task<UserModel?> Find(int id);
  Task<List<UserModel>> FindAll();
  Task<UserModel> Update(UserModel card);
  Task Remove(int id);
}