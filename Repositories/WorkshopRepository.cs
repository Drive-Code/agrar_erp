using MySqlConnector;
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Repositories
{
    public class Workshop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class WorkshopRepository
    {
        private readonly string connectionString;

        public WorkshopRepository()
        {
            connectionString = DatabaseConnection.ConnectionString;
        }

        private Workshop Map(MySqlDataReader reader)
        {
            int idOrd = reader.GetOrdinal("id");
            int nameOrd = reader.GetOrdinal("name");
            int descOrd = reader.GetOrdinal("description");

            return new Workshop
            {
                Id = reader.GetInt32(idOrd),
                Name = reader.GetString(nameOrd),
                Description = reader.IsDBNull(descOrd) ? null : reader.GetString(descOrd)
            };
        }

        public List<Workshop> GetAll()
        {
            var list = new List<Workshop>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, name, description FROM workshops ORDER BY name";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(Map(reader));
                }
            }
            return list;
        }

        public Workshop GetById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, name, description FROM workshops WHERE id = @id";
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

        public int Insert(Workshop workshop)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO workshops (name, description) VALUES (@name, @desc); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", workshop.Name);
                    cmd.Parameters.AddWithValue("@desc", (object)workshop.Description ?? DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Workshop workshop)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE workshops SET name = @name, description = @desc WHERE id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", workshop.Id);
                    cmd.Parameters.AddWithValue("@name", workshop.Name);
                    cmd.Parameters.AddWithValue("@desc", (object)workshop.Description ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM workshops WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Workshop> GetPage(int page, int pageSize)
        {
            var list = new List<Workshop>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, name, description FROM workshops ORDER BY name LIMIT @limit OFFSET @offset";
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
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM workshops", conn))
                    return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<Workshop> SearchPage(string searchText, int page, int pageSize)
        {
            var list = new List<Workshop>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, name, description FROM workshops 
                               WHERE name LIKE @search OR description LIKE @search 
                               ORDER BY name 
                               LIMIT @limit OFFSET @offset";
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
                string sql = "SELECT COUNT(*) FROM workshops WHERE name LIKE @search OR description LIKE @search";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}