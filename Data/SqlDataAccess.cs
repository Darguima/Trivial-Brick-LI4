using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace TrivialBrick.Data;

public class SqlDataAccess(IConfiguration config) : ISqlDataAccess
{
  private readonly IConfiguration _config = config;

  public string ConnectionStringName { get; set; } = "TrivialBrickDB";

    public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
  {
    string? connectionString = _config.GetConnectionString(ConnectionStringName);
    using (IDbConnection connection = new SqlConnection(connectionString))
    {
      var data = await connection.QueryAsync<T>(sql, parameters);
      return data.ToList();
    }
  }

  public async Task SaveData<T>(string sql, T parameters)
  {
    string? connectionString = _config.GetConnectionString(ConnectionStringName);
    using (IDbConnection connection = new SqlConnection(connectionString))
    {
      await connection.ExecuteAsync(sql, parameters);
    }
  }
}