using MySqlConnector;
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Repositories
{
    public class DirectoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconName { get; set; }
        public string TableName { get; set; }
    }

    public class DirectoryRepository
    {
        private string connectionString;

        public DirectoryRepository(string connString)
        {
            connectionString = connString;
        }

        public List<DirectoryItem> GetAll()
        {
            var list = new List<DirectoryItem>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT id, name, description, icon_name, table_name FROM directories ORDER BY name", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DirectoryItem
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            IconName = reader.IsDBNull(3) ? null : reader.GetString(3),
                            TableName = reader.IsDBNull(4) ? null : reader.GetString(4)
                        });
                    }
                }
            }
            return list;
        }

        public DirectoryItem GetById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT id, name, description, icon_name, table_name FROM directories WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new DirectoryItem
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                IconName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                TableName = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public int Insert(DirectoryItem item)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    "INSERT INTO directories (name, description, icon_name, table_name) VALUES (@name, @desc, @icon, @table); SELECT LAST_INSERT_ID();", conn))
                {
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@desc", (object)item.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@icon", (object)item.IconName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@table", (object)item.TableName ?? DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(DirectoryItem item)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    "UPDATE directories SET name=@name, description=@desc, icon_name=@icon, table_name=@table WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", item.Id);
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@desc", (object)item.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@icon", (object)item.IconName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@table", (object)item.TableName ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM directories WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}