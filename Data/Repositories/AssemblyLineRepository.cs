using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class AssemblyLineRepository(ISqlDataAccess db)
    {
        private readonly ISqlDataAccess _db = db;

        public async Task<AssemblyLine?> Find(string id)
        {
            string sql = "select * from assembly_lines where assembly_line_id = @id";
            var assemblyLines = await _db.LoadData<AssemblyLine, dynamic>(sql, new { id });
            return assemblyLines?.FirstOrDefault();
        }

        public Task<List<AssemblyLine>> FindAll()
        {
            string sql = "select * from assembly_lines";
            return _db.LoadData<AssemblyLine, dynamic>(sql, new { });
        }

        public async Task<AssemblyLine> Add(string ID, AssemblyLineState state = AssemblyLineState.Inactive, int? orderId = null)
        {
            string sql = "insert into assembly_lines (assembly_line_id, state, order_id) values (@ID, @State, @OrderId)";
            await _db.SaveData(sql, new { ID, State = state.ToString(), OrderId = orderId });
            return new AssemblyLine { Assembly_line_id = ID, State = state, Order_id = orderId };
        }

        public Task Update(AssemblyLine assemblyLine)
        {
            string sql = "update assembly_lines set state = @State, order_id = @OrderId where assembly_line_id = @ID";
            return _db.SaveData(sql, new { ID = assemblyLine.Assembly_line_id, State = assemblyLine.State.ToString(), OrderId = assemblyLine.Order_id });
        }

        public Task Remove(AssemblyLine assemblyLine)
        {
            string sql = "delete from assembly_lines where assembly_line_id = @ID";
            return _db.SaveData(sql, new { ID = assemblyLine.Assembly_line_id });
        }

        public async Task<List<AssemblyLine>> FindAllActiveAndFree()
        {
            string sql = "select * from assembly_lines where state = 'Active' and order_id is null";
            return await _db.LoadData<AssemblyLine, dynamic>(sql, new { });
        }
    }
}