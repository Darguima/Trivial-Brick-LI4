using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrivialBrick.DataLayer;

namespace TrivialBrick.DataLayer;

public class UserRepository : IUserRepository
{
  private ISqlDataAccess _db;

  public UserRepository(ISqlDataAccess db)
  {
    _db = db;
  }

  public async Task<UserModel?> Find(int id)
  {
    string sql = "select * from Utilizador where ID = @id";
    var users = await _db.LoadData<UserModel, dynamic>(sql, new { id });
    return users?.FirstOrDefault();
  }

  public Task<List<UserModel>> FindAll()
  {
    string sql = "select * from Utilizador";
    return _db.LoadData<UserModel, dynamic>(sql, new { });
  }

  public Task<UserModel> Update(UserModel user)
  {
    throw new NotImplementedException();
  }

  public Task Remove(int id)
  {
    throw new NotImplementedException();
  }
}