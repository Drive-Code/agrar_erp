using MySqlConnector;
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Repositories
{
    public class Operation
    {
        public int Id { get; set; }
        public int? MaterialId { get; set; }
        public int? EmployeeId { get; set; }
        public int? WorkshopId { get; set; }
        public int? ProductId { get; set; }
        public string OperationType { get; set; }
        public int? Quantity { get; set; }
        public decimal? MaterialQuantity { get; set; }
        public DateTime? OperationDate { get; set; }
    }

    public class OperationRepository
    {
        private readonly string connectionString;

        public OperationRepository()
        {
            connectionString = DatabaseConnection.ConnectionString;
        }

        private Operation Map(MySqlDataReader reader)
        {
            int idOrd = reader.GetOrdinal("id");
            int matOrd = reader.GetOrdinal("material_id");
            int empOrd = reader.GetOrdinal("employee_id");
            int workshopOrd = reader.GetOrdinal("workshop_id");
            int productOrd = reader.GetOrdinal("product_id");
            int typeOrd = reader.GetOrdinal("operation_type");
            int qtyOrd = reader.GetOrdinal("quantity");
            int matQtyOrd = reader.GetOrdinal("material_quantity");
            int dateOrd = reader.GetOrdinal("operation_date");

            return new Operation
            {
                Id = reader.GetInt32(idOrd),
                MaterialId = reader.IsDBNull(matOrd) ? (int?)null : reader.GetInt32(matOrd),
                EmployeeId = reader.IsDBNull(empOrd) ? (int?)null : reader.GetInt32(empOrd),
                WorkshopId = reader.IsDBNull(workshopOrd) ? (int?)null : reader.GetInt32(workshopOrd),
                ProductId = reader.IsDBNull(productOrd) ? (int?)null : reader.GetInt32(productOrd),
                OperationType = reader.IsDBNull(typeOrd) ? null : reader.GetString(typeOrd),
                Quantity = reader.IsDBNull(qtyOrd) ? (int?)null : reader.GetInt32(qtyOrd),
                MaterialQuantity = reader.IsDBNull(matQtyOrd) ? (decimal?)null : reader.GetDecimal(matQtyOrd),
                OperationDate = reader.IsDBNull(dateOrd) ? (DateTime?)null : reader.GetDateTime(dateOrd)
            };
        }

        public List<Operation> GetAll()
        {
            var list = new List<Operation>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, material_id, employee_id, workshop_id, product_id, operation_type, quantity, material_quantity, operation_date FROM operations ORDER BY id";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(Map(reader));
                }
            }
            return list;
        }

        public Operation GetById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, material_id, employee_id, workshop_id, product_id, operation_type, quantity, material_quantity, operation_date FROM operations WHERE id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return Map(reader);
                    }
                }
            }
            return null;
        }

        public int Insert(Operation operation)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO operations (material_id, employee_id, workshop_id, product_id, operation_type, quantity, material_quantity, operation_date)
                               VALUES (@matId, @empId, @workshopId, @productId, @type, @qty, @matQty, @opDate);
                               SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@matId", (object)operation.MaterialId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@empId", (object)operation.EmployeeId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@workshopId", (object)operation.WorkshopId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@productId", (object)operation.ProductId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@type", (object)operation.OperationType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@qty", (object)operation.Quantity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@matQty", (object)operation.MaterialQuantity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@opDate", (object)operation.OperationDate ?? DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Operation operation)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE operations SET material_id=@matId, employee_id=@empId, workshop_id=@workshopId, product_id=@productId,
                               operation_type=@type, quantity=@qty, material_quantity=@matQty, operation_date=@opDate WHERE id=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", operation.Id);
                    cmd.Parameters.AddWithValue("@matId", (object)operation.MaterialId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@empId", (object)operation.EmployeeId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@workshopId", (object)operation.WorkshopId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@productId", (object)operation.ProductId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@type", (object)operation.OperationType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@qty", (object)operation.Quantity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@matQty", (object)operation.MaterialQuantity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@opDate", (object)operation.OperationDate ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM operations WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Operation> GetPage(int page, int pageSize)
        {
            var list = new List<Operation>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, material_id, employee_id, workshop_id, product_id, operation_type, quantity, material_quantity, operation_date FROM operations ORDER BY id LIMIT @limit OFFSET @offset";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@limit", pageSize);
                    cmd.Parameters.AddWithValue("@offset", offset);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(Map(reader));
                    }
                }
            }
            return list;
        }

        public int GetTotalCount()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM operations", conn))
                    return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<Operation> SearchPage(string searchText, int page, int pageSize)
        {
            var list = new List<Operation>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, material_id, employee_id, workshop_id, product_id, operation_type, quantity, material_quantity, operation_date FROM operations
                               WHERE operation_type LIKE @search
                               ORDER BY id LIMIT @limit OFFSET @offset";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    cmd.Parameters.AddWithValue("@limit", pageSize);
                    cmd.Parameters.AddWithValue("@offset", offset);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(Map(reader));
                    }
                }
            }
            return list;
        }

        public int GetSearchTotalCount(string searchText)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM operations WHERE operation_type LIKE @search";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}