using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class AdminRepository(ISqlDataAccess db)
    {
        private readonly ISqlDataAccess _db = db;

        public async Task<Admin?> Find(string id)
        {
            string sql = "select * from Users as u inner join Admins as a on u.ID = a.user_id where u.ID = @id";
            var admin = await _db.LoadData<Admin, dynamic>(sql, new { id });
            return admin?.FirstOrDefault();
        }

        public async Task<Admin?> FindByMailPassword(string mail, string password)
        {
            string sql = "select * from users as u inner join Admins as a on u.ID = a.user_id where Mail = @mail and Password = @password";
            var admin = await _db.LoadData<Admin, dynamic>(sql, new { mail, password });
            return admin.FirstOrDefault();
        }

        public Task<List<Admin>> FindAll()
        {
            string sql = "select * from Users as u inner join Admins as a on u.ID = a.user_id";
            return _db.LoadData<Admin, dynamic>(sql, new { });
        }

        public async Task<Admin> Add(string name, string email, string password)
        {
            string sql = @"
            insert into Users (name, mail, password) values (@name, @email, @password);
            select SCOPE_IDENTITY();
        ";
            var userId = await _db.LoadData<int, dynamic>(sql, new { name, email, password });
            int id = userId.FirstOrDefault();

            sql = "insert into Admins (user_id) values (@id)";
            await _db.SaveData(sql, new { id });

            var dbAdmin = await Find(id.ToString()) ?? throw new Exception("Some unexpected error occurred while creating the client");

            return dbAdmin;
        }

        public Task Update(Admin client)
        {
            string sql = "update Users set Name = @Name, Mail = @Mail, Password = @Password where ID = @ID;";
            return _db.SaveData(sql, client);
        }

        public Task Remove(Admin client)
        {
            string sql = "delete from Admins where User_ID = @ID; delete from Users where ID = @ID";
            return _db.SaveData(sql, client);
        }
    }
}