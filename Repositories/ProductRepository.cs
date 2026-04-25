using MySqlConnector;
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Repositories
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ProductionDate { get; set; }
    }

    public class ProductRepository
    {
        private readonly string connectionString;

        public ProductRepository()
        {
            connectionString = DatabaseConnection.ConnectionString;
        }

        private Product Map(MySqlDataReader reader)
        {
            int idOrd = reader.GetOrdinal("id");
            int nameOrd = reader.GetOrdinal("name");
            int articleOrd = reader.GetOrdinal("article");
            int qtyOrd = reader.GetOrdinal("quantity");
            int prodDateOrd = reader.GetOrdinal("production_date");

            return new Product
            {
                Id = reader.GetInt32(idOrd),
                Name = reader.GetString(nameOrd),
                Article = reader.IsDBNull(articleOrd) ? null : reader.GetString(articleOrd),
                Quantity = reader.IsDBNull(qtyOrd) ? (int?)null : reader.GetInt32(qtyOrd),
                ProductionDate = reader.IsDBNull(prodDateOrd) ? (DateTime?)null : reader.GetDateTime(prodDateOrd)
            };
        }

        public List<Product> GetAll()
        {
            var list = new List<Product>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, name, article, quantity, production_date FROM products ORDER BY name";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(Map(reader));
                }
            }
            return list;
        }

        public Product GetById(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, name, article, quantity, production_date FROM products WHERE id = @id";
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

        public int Insert(Product product)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO products (name, article, quantity, production_date)
                               VALUES (@name, @article, @qty, @prodDate);
                               SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@article", (object)product.Article ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@qty", (object)product.Quantity ?? DBNull.Value);
                    object prodDateParam = (product.ProductionDate.HasValue && product.ProductionDate.Value != DateTime.MinValue)
                        ? (object)product.ProductionDate.Value
                        : DBNull.Value;
                    cmd.Parameters.AddWithValue("@prodDate", prodDateParam);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Product product)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE products SET name=@name, article=@article, quantity=@qty, production_date=@prodDate
                               WHERE id=@id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", product.Id);
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@article", (object)product.Article ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@qty", (object)product.Quantity ?? DBNull.Value);
                    object prodDateParam = (product.ProductionDate.HasValue && product.ProductionDate.Value != DateTime.MinValue)
                        ? (object)product.ProductionDate.Value
                        : DBNull.Value;
                    cmd.Parameters.AddWithValue("@prodDate", prodDateParam);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM products WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetPage(int page, int pageSize)
        {
            var list = new List<Product>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT id, name, article, quantity, production_date FROM products ORDER BY name LIMIT @limit OFFSET @offset";
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
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM products", conn))
                    return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<Product> SearchPage(string searchText, int page, int pageSize)
        {
            var list = new List<Product>();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT id, name, article, quantity, production_date FROM products
                               WHERE name LIKE @search OR article LIKE @search
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
                string sql = "SELECT COUNT(*) FROM products WHERE name LIKE @search OR article LIKE @search";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}