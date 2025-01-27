using TrivialBrick.Data.Models;

namespace TrivialBrick.Data.Repositories
{
    public class InvoiceRepository
    {
        private readonly ISqlDataAccess _db;

        public InvoiceRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Invoice?> Find(int id)
        {
            string sql = "select * from invoices where invoice_id = @id";
            var invoices = await _db.LoadData<Invoice, dynamic>(sql, new { id });
            return invoices?.FirstOrDefault();
        }

        public Task<List<Invoice>> FindAll()
        {
            string sql = "select * from invoices";
            return _db.LoadData<Invoice, dynamic>(sql, new { });
        }

        public async Task<Invoice> Add(DateTime datetime, int client_id, int order_id)
        {
            string sql = @"
                insert into invoices (datetime, client_id, order_id) 
                values (@datetime, @client_id, @order_id);
                select SCOPE_IDENTITY();
            ";
            var invoiceId = await _db.LoadData<int, dynamic>(sql, new { datetime, client_id, order_id });
            int id = invoiceId.FirstOrDefault();

            Invoice? invoice = await Find(id);

            if (invoice == null)
            {
                throw new Exception("Some unexpected error occurred while creating the invoice");
            }

            return invoice;
        }

        public Task Update(Invoice invoice)
        {
            string sql = "update invoices set datetime = @Datetime, client_id = @Client_id, order_id = @Order_id where invoice_id = @Invoice_id";
            return _db.SaveData(sql, invoice);
        }

        public Task Remove(Invoice invoice)
        {
            string sql = "delete from invoices where invoice_id = @Invoice_id";
            return _db.SaveData(sql, invoice);
        }

        public async Task <Invoice?> FindByOrder(int order_id)
        {
            string sql = "select * from invoices where order_id = @order_id";
            var invoices = await _db.LoadData<Invoice, dynamic>(sql, new { order_id });
            return invoices?.FirstOrDefault();
        }
    }
}