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

        public async Task<AssemblyLine> Add(string ID, AssemblyLineState state = AssemblyLineState.Inactive, int? orderId = null, DateTime? startMountTime = null, DateTime? endMountTime = null)
        {
            string sql = "insert into assembly_lines (assembly_line_id, state, order_id, mount_start_time, expected_end_time) values (@ID, @State, @OrderId, @StartMountTime, @EndMountTime)";
            await _db.SaveData(sql, new { ID, State = state.ToString(), OrderId = orderId, StartMountTime = startMountTime, EndMountTime = endMountTime });
            return new AssemblyLine { Assembly_line_id = ID, State = state, Order_id = orderId, Mount_start_time = startMountTime, Expected_end_time = endMountTime };
        }

        public Task Update(AssemblyLine assemblyLine)
        {
            string sql = "update assembly_lines set state = @State, order_id = @OrderId, mount_start_time = @StartMountTime, expected_end_time = @EndMountTime where assembly_line_id = @ID";
            return _db.SaveData(sql, new
            {
                ID = assemblyLine.Assembly_line_id,
                State = assemblyLine.State.ToString(),
                OrderId = assemblyLine.Order_id,
                StartMountTime = assemblyLine.Mount_start_time,
                EndMountTime = assemblyLine.Expected_end_time
            });
        }

        public Task Remove(AssemblyLine assemblyLine)
        {
            string sql = "delete from assembly_lines where assembly_line_id = @ID";
            return _db.SaveData(sql, new { ID = assemblyLine.Assembly_line_id });
        }

        public async Task<List<AssemblyLine>> FindAllActiveAndFree()
        {
            string sql = "select * from assembly_lines where state = 'active' and order_id is null";
            return await _db.LoadData<AssemblyLine, dynamic>(sql, new { });
        }

        public async Task<List<AssemblyLine>> FindAllOccupied()
        {
            string sql = "select * from assembly_lines where order_id is not null";
            return await _db.LoadData<AssemblyLine, dynamic>(sql, new { });
        }

        public async Task<AssemblyLine?> GetLineByOrder(int orderId)
        {
            string sql = "select * from assembly_lines where order_id = @orderId";
            var assemblyLines = await _db.LoadData<AssemblyLine, dynamic>(sql, new { orderId });
            return assemblyLines?.FirstOrDefault();
        }
    }
}