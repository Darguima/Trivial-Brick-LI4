using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class NotificationRepository
    {
        private readonly ISqlDataAccess _db;

        public NotificationRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Notification?> Find(int id)
        {
            string sql = "select * from notifications where notification_id = @id";
            var notifications = await _db.LoadData<Notification, dynamic>(sql, new { id });
            return notifications?.FirstOrDefault();
        }

        public Task<List<Notification>> FindAll()
        {
            string sql = "select * from notifications";
            return _db.LoadData<Notification, dynamic>(sql, new { });
        }

        public async Task<Notification> Add(string message, DateTime datetime, int client_id, int order_id)
        {
            string sql = @"
                insert into notifications (message, datetime, client_id, order_id) 
                values (@message, @datetime, @client_id, @order_id);
                select SCOPE_IDENTITY();
            ";
            var notificationId = await _db.LoadData<int, dynamic>(sql, new { message, datetime, client_id, order_id });
            int id = notificationId.FirstOrDefault();

            Notification? notification = await Find(id);

            if (notification == null)
            {
                throw new Exception("Some unexpected error occurred while creating the notification");
            }

            return notification;
        }

        public Task Update(Notification notification)
        {
            string sql = "update notifications set message = @Message, datetime = @Datetime, client_id = @Client_id, order_id = @Order_id where notification_id = @Notification_id";
            return _db.SaveData(sql, notification);
        }

        public Task Remove(Notification notification)
        {
            string sql = "delete from notifications where notification_id = @Notification_id";
            return _db.SaveData(sql, notification);
        }
    }
}