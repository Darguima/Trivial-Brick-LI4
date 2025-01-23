using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class PartRepository
    {
        private readonly ISqlDataAccess _db;

        public PartRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Part?> FindPart(int id)
        {
            string sql = "select * from parts where part_id = @id";
            var parts = await _db.LoadData<Part, dynamic>(sql, new { id });
            return parts?.FirstOrDefault();
        }

        public Task<List<Part>> FindAllParts()
        {
            string sql = "select * from parts";
            return _db.LoadData<Part, dynamic>(sql, new { });
        }

        public async Task<Part> AddPart(int partId, string image, int stock)
        {
            string sql = @"
                insert into parts (part_id, image, stock) 
                values (@partId, @image, @stock);
            ";
            await _db.SaveData(sql, new { partId, image, stock });

            Part? part = await FindPart(partId);

            if (part == null)
            {
                throw new Exception("Some unexpected error occurred while creating the part");
            }

            return part;
        }

        public Task UpdatePart(Part part)
        {
            string sql = "update parts set image = @Image, stock = @Stock where part_id = @Part_id";
            return _db.SaveData(sql, part);
        }

        public Task RemovePart(Part part)
        {
            string sql = "delete from parts where part_id = @Part_id";
            return _db.SaveData(sql, part);
        }
    }
}