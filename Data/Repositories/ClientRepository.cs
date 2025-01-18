using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class ClientRepository(ISqlDataAccess db)
    {
        private readonly ISqlDataAccess _db = db;

        public async Task<Client?> Find(string id)
        {
            string sql = "select * from Users as u inner join Clients as c on u.ID = c.user_id where u.ID = @id";
            var clients = await _db.LoadData<Client, dynamic>(sql, new { id });
            return clients?.FirstOrDefault();
        }

        public async Task<Client?> FindByMailPassword(string mail, string password)
        {
            string sql = "select * from users as u inner join clients as c on u.ID = c.user_id where Mail = @mail and Password = @password";
            var client = await _db.LoadData<Client, dynamic>(sql, new { mail, password });
            return client.FirstOrDefault();
        }

        public Task<List<Client>> FindAll()
        {
            string sql = "select * from Users as u inner join Clients as c on u.ID = c.user_id";
            return _db.LoadData<Client, dynamic>(sql, new { });
        }

        public async Task<Client> Add(string name, string email, string password, string nif)
        {
            string sql = @"
            insert into Users (name, mail, password) values (@name, @email, @password);
            select SCOPE_IDENTITY();
        ";
            var userId = await _db.LoadData<int, dynamic>(sql, new { name, email, password });
            int id = userId.FirstOrDefault();

            sql = "insert into Clients (user_id, nif) values (@id, @nif)";
            await _db.SaveData(sql, new { id, nif });

            var dbClient = await Find(id.ToString()) ?? throw new Exception("Some unexpected error occurred while creating the client");

            return dbClient;
        }

        public Task Update(Client client)
        {
            string sql = "update Users set Name = @Name, Mail = @Mail, Password = @Password where ID = @ID; update Clients set NIF = @NIF where User_ID = @ID";
            return _db.SaveData(sql, client);
        }

        public Task Remove(Client client)
        {
            string sql = "delete from Clients where User_ID = @ID; delete from Users where ID = @ID";
            return _db.SaveData(sql, client);
        }
    }
}