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

  public async Task<User?> Find(int id)
  {
    string sql = "select * from Users where ID = @id";
    var users = await _db.LoadData<User, dynamic>(sql, new { id });
    return users?.FirstOrDefault();
  }

  public Task<List<User>> FindAll()
  {
    string sql = "select * from Users";
    return _db.LoadData<User, dynamic>(sql, new { });
  }

  public Task<User> Update(User user)
  {
    throw new NotImplementedException();
  }

  public Task Remove(int id)
  {
    throw new NotImplementedException();
  }
}