using MySqlConnector;
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Repositories
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
    }

    public class SupplierRepository
    {
        private readonly string connectionString;

        public SupplierRepository()
        {
            connectionString = DatabaseConnection.ConnectionString;
        }

        private Supplier Map(MySqlDataReader reader)
        {
            int idOrd = reader.GetOrdinal("id");
            int nameOrd = reader.GetOrdinal("name");
            int contactOrd = reader.GetOrdinal("contact_person");
            int phoneOrd = reader.GetOrdinal("phone");

            return new Supplier
            {
                Id = reader.GetInt32(idOrd),
                Name = reader.GetString(nameOrd),
                ContactPerson = reader.IsDBNull(contactOrd) ? null : reader.GetString(contactOrd),
                Phone = reader.IsDBNull(phoneOrd) ? null : reader.GetString(phoneOrd)
            };
        }

        public List<Supplier> GetAll()
        {
            var list = new List<Supplier>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, name, contact_person, phone FROM suppliers ORDER BY name";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(Map(reader));
                }
            }
            return list;
        }

        public Supplier GetById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, name, contact_person, phone FROM suppliers WHERE id = @id";
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

        public int Insert(Supplier supplier)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO suppliers (name, contact_person, phone)
                               VALUES (@name, @contact, @phone);
                               SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", supplier.Name);
                    cmd.Parameters.AddWithValue("@contact", (object)supplier.ContactPerson ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@phone", (object)supplier.Phone ?? DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Supplier supplier)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE suppliers SET name=@name, contact_person=@contact, phone=@phone
                               WHERE id=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", supplier.Id);
                    cmd.Parameters.AddWithValue("@name", supplier.Name);
                    cmd.Parameters.AddWithValue("@contact", (object)supplier.ContactPerson ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@phone", (object)supplier.Phone ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM suppliers WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Supplier> GetPage(int page, int pageSize)
        {
            var list = new List<Supplier>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, name, contact_person, phone FROM suppliers ORDER BY name LIMIT @limit OFFSET @offset";
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
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM suppliers", conn))
                    return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<Supplier> SearchPage(string searchText, int page, int pageSize)
        {
            var list = new List<Supplier>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, name, contact_person, phone FROM suppliers
                               WHERE name LIKE @search OR contact_person LIKE @search OR phone LIKE @search
                               ORDER BY name LIMIT @limit OFFSET @offset";
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
                string sql = @"SELECT COUNT(*) FROM suppliers
                               WHERE name LIKE @search OR contact_person LIKE @search OR phone LIKE @search";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}