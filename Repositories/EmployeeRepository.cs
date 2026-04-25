using MySqlConnector;
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Repositories
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? WorkshopId { get; set; }
    }

    public class EmployeeRepository
    {
        private readonly string connectionString;

        public EmployeeRepository()
        {
            connectionString = DatabaseConnection.ConnectionString;
        }

        private Employee Map(MySqlDataReader reader)
        {
            int idOrd = reader.GetOrdinal("id");
            int nameOrd = reader.GetOrdinal("full_name");
            int posOrd = reader.GetOrdinal("position");
            int phoneOrd = reader.GetOrdinal("phone");
            int emailOrd = reader.GetOrdinal("email");
            int workshopOrd = reader.GetOrdinal("workshop_id");

            return new Employee
            {
                Id = reader.GetInt32(idOrd),
                FullName = reader.GetString(nameOrd),
                Position = reader.IsDBNull(posOrd) ? null : reader.GetString(posOrd),
                Phone = reader.IsDBNull(phoneOrd) ? null : reader.GetString(phoneOrd),
                Email = reader.IsDBNull(emailOrd) ? null : reader.GetString(emailOrd),
                WorkshopId = reader.IsDBNull(workshopOrd) ? (int?)null : reader.GetInt32(workshopOrd)
            };
        }

        public List<Employee> GetAll()
        {
            var list = new List<Employee>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, full_name, position, phone, email, workshop_id FROM employees ORDER BY full_name";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(Map(reader));
                }
            }
            return list;
        }

        public Employee GetById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, full_name, position, phone, email, workshop_id FROM employees WHERE id = @id";
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

        public int Insert(Employee employee)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO employees (full_name, position, phone, email, workshop_id)
                               VALUES (@name, @pos, @phone, @email, @workshopId);
                               SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", employee.FullName);
                    cmd.Parameters.AddWithValue("@pos", (object)employee.Position ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@phone", (object)employee.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", (object)employee.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@workshopId", (object)employee.WorkshopId ?? DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Employee employee)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE employees SET full_name=@name, position=@pos, phone=@phone, email=@email, workshop_id=@workshopId
                               WHERE id=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", employee.Id);
                    cmd.Parameters.AddWithValue("@name", employee.FullName);
                    cmd.Parameters.AddWithValue("@pos", (object)employee.Position ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@phone", (object)employee.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", (object)employee.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@workshopId", (object)employee.WorkshopId ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM employees WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Employee> GetPage(int page, int pageSize)
        {
            var list = new List<Employee>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, full_name, position, phone, email, workshop_id FROM employees ORDER BY full_name LIMIT @limit OFFSET @offset";
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
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM employees", conn))
                    return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<Employee> SearchPage(string searchText, int page, int pageSize)
        {
            var list = new List<Employee>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, full_name, position, phone, email, workshop_id FROM employees
                               WHERE full_name LIKE @search OR position LIKE @search OR phone LIKE @search OR email LIKE @search
                               ORDER BY full_name LIMIT @limit OFFSET @offset";
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
                string sql = @"SELECT COUNT(*) FROM employees
                               WHERE full_name LIKE @search OR position LIKE @search OR phone LIKE @search OR email LIKE @search";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}