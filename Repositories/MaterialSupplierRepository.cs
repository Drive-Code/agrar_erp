using MySqlConnector;
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Repositories
{
    public class MaterialSupplier
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public int SupplierId { get; set; }
        public decimal? Quantity { get; set; }
        public DateTime? SupplyDate { get; set; }
        // Для отображения
        public string MaterialName { get; set; }
        public string SupplierName { get; set; }
    }

    public class MaterialSupplierRepository
    {
        private readonly string connectionString;

        public MaterialSupplierRepository()
        {
            connectionString = DatabaseConnection.ConnectionString;
        }

        public List<MaterialSupplier> GetAll()
        {
            var list = new List<MaterialSupplier>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT ms.id, ms.material_id, ms.supplier_id, ms.quantity, ms.supply_date,
                                      m.name AS material_name, s.name AS supplier_name
                               FROM material_suppliers ms
                               JOIN materials m ON ms.material_id = m.id
                               JOIN suppliers s ON ms.supplier_id = s.id
                               ORDER BY m.name, s.name";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    int idOrd = reader.GetOrdinal("id");
                    int matIdOrd = reader.GetOrdinal("material_id");
                    int supIdOrd = reader.GetOrdinal("supplier_id");
                    int qtyOrd = reader.GetOrdinal("quantity");
                    int dateOrd = reader.GetOrdinal("supply_date");
                    int matNameOrd = reader.GetOrdinal("material_name");
                    int supNameOrd = reader.GetOrdinal("supplier_name");

                    while (reader.Read())
                    {
                        list.Add(new MaterialSupplier
                        {
                            Id = reader.GetInt32(idOrd),
                            MaterialId = reader.GetInt32(matIdOrd),
                            SupplierId = reader.GetInt32(supIdOrd),
                            Quantity = reader.IsDBNull(qtyOrd) ? (decimal?)null : reader.GetDecimal(qtyOrd),
                            SupplyDate = reader.IsDBNull(dateOrd) ? (DateTime?)null : reader.GetDateTime(dateOrd),
                            MaterialName = reader.GetString(matNameOrd),
                            SupplierName = reader.GetString(supNameOrd)
                        });
                    }
                }
            }
            return list;
        }

        public void Insert(int materialId, int supplierId, decimal? quantity, DateTime? supplyDate)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO material_suppliers (material_id, supplier_id, quantity, supply_date)
                               VALUES (@matId, @supId, @qty, @date)
                               ON DUPLICATE KEY UPDATE quantity=@qty, supply_date=@date";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@matId", materialId);
                    cmd.Parameters.AddWithValue("@supId", supplierId);
                    cmd.Parameters.AddWithValue("@qty", (object)quantity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@date", (object)supplyDate ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM material_suppliers WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}