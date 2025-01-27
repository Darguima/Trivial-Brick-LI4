using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class InstructionRepository
    {
        private readonly ISqlDataAccess _db;

        public InstructionRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Instruction?> FindInstruction(int productId, int seqNum)
        {
            string sql = "select * from instructions where product_id = @productId and seq_num = @seq_num";
            var instructions = await _db.LoadData<Instruction, dynamic>(sql, new { productId, seqNum });
            return instructions?.FirstOrDefault();
        }

        public Task<List<Instruction>> FindAllInstructions()
        {
            string sql = "select * from instructions";
            return _db.LoadData<Instruction, dynamic>(sql, new { });
        }

        public async Task<Instruction> AddInstruction(int productId, int seqNum, string image, int qntParts)
        {
            string sql = "insert into instructions (product_id, seq_num, image, qnt_parts) values (@productId, @seqNum, @image, @qntParts)";
            await _db.SaveData(sql, new { productId, seqNum, image, qntParts });
            return new Instruction { Product_id = productId, Seq_num = seqNum, Image = image, Qnt_parts = qntParts };
        }

        public Task UpdateInstruction(Instruction instruction)
        {
            string sql = "update instructions set image = @Image, quantity = @Qnt_parts where part_id = @Part_id and product_id = @Product_id";
            return _db.SaveData(sql, instruction);
        }

        public Task RemoveInstruction(Instruction instruction)
        {
            string sql = "delete from instructions where part_id = @Part_id and product_id = @Product_id";
            return _db.SaveData(sql, instruction);
        }
    }
}