using MySqlConnector;
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Repositories
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BatchNumber { get; set; }
        public decimal? FullBatchMass { get; set; }
        public decimal? TotalMass { get; set; }
        public decimal? Length { get; set; }
        public int? Quantity { get; set; }
        public string ArrivalFrom { get; set; }
        public string ConsumptionTo { get; set; }
        public DateTime? ArrivalDate { get; set; }
    }

    public class MaterialRepository
    {
        private readonly string connectionString;

        public MaterialRepository()
        {
            connectionString = DatabaseConnection.ConnectionString;
        }

        public MaterialRepository(string connString)
        {
            connectionString = connString;
        }

        public List<Material> GetAll()
        {
            var list = new List<Material>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, name, batch_number, full_batch_mass, total_mass, length, quantity, 
                                      arrival_from, consumption_to, arrival_date 
                               FROM materials ORDER BY id";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(Map(reader));
                }
            }
            return list;
        }

        public Material GetById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, name, batch_number, full_batch_mass, total_mass, length, quantity, 
                                      arrival_from, consumption_to, arrival_date 
                               FROM materials WHERE id = @id";
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

        public int Insert(Material material)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO materials 
                    (name, batch_number, full_batch_mass, total_mass, length, quantity, arrival_from, consumption_to, arrival_date)
                    VALUES (@name, @batch, @fullMass, @totalMass, @length, @qty, @arrival, @consumption, @arrivalDate);
                    SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    AddParameters(cmd, material);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Material material)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE materials SET 
                    name=@name, batch_number=@batch, full_batch_mass=@fullMass, total_mass=@totalMass,
                    length=@length, quantity=@qty, arrival_from=@arrival, consumption_to=@consumption,
                    arrival_date=@arrivalDate
                    WHERE id=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", material.Id);
                    AddParameters(cmd, material);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM materials WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Material> GetPage(int page, int pageSize)
        {
            var list = new List<Material>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, name, batch_number, full_batch_mass, total_mass, length, quantity, 
                                      arrival_from, consumption_to, arrival_date 
                               FROM materials ORDER BY id LIMIT @limit OFFSET @offset";
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
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM materials", conn))
                    return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<Material> SearchPage(string searchText, int page, int pageSize)
        {
            var list = new List<Material>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, name, batch_number, full_batch_mass, total_mass, length, quantity, 
                                      arrival_from, consumption_to, arrival_date 
                               FROM materials 
                               WHERE name LIKE @search OR batch_number LIKE @search OR arrival_from LIKE @search
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
                string sql = @"SELECT COUNT(*) FROM materials 
                               WHERE name LIKE @search OR batch_number LIKE @search OR arrival_from LIKE @search";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private Material Map(MySqlDataReader reader)
        {
            return new Material
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                BatchNumber = reader.IsDBNull(reader.GetOrdinal("batch_number")) ? null : reader.GetString(reader.GetOrdinal("batch_number")),
                FullBatchMass = reader.IsDBNull(reader.GetOrdinal("full_batch_mass")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("full_batch_mass")),
                TotalMass = reader.IsDBNull(reader.GetOrdinal("total_mass")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("total_mass")),
                Length = reader.IsDBNull(reader.GetOrdinal("length")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("length")),
                Quantity = reader.IsDBNull(reader.GetOrdinal("quantity")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("quantity")),
                ArrivalFrom = reader.IsDBNull(reader.GetOrdinal("arrival_from")) ? null : reader.GetString(reader.GetOrdinal("arrival_from")),
                ConsumptionTo = reader.IsDBNull(reader.GetOrdinal("consumption_to")) ? null : reader.GetString(reader.GetOrdinal("consumption_to")),
                ArrivalDate = reader.IsDBNull(reader.GetOrdinal("arrival_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("arrival_date"))
            };
        }

        private void AddParameters(MySqlCommand cmd, Material m)
        {
            cmd.Parameters.AddWithValue("@name", m.Name);
            cmd.Parameters.AddWithValue("@batch", (object)m.BatchNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@fullMass", (object)m.FullBatchMass ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@totalMass", (object)m.TotalMass ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@length", (object)m.Length ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@qty", (object)m.Quantity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@arrival", (object)m.ArrivalFrom ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@consumption", (object)m.ConsumptionTo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@arrivalDate", (object)m.ArrivalDate ?? DBNull.Value);
        }
    }
}