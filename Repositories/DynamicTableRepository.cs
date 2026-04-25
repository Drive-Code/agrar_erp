using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace WindowsFormsApp1.Repositories
{
    public class ColumnDefinition
    {
        public string Name { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public int? Length { get; set; }
        public bool IsNullable { get; set; }
        public bool IsForeignKey { get; set; }
        public string ReferenceTablesString { get; set; }
        public string ReferenceHeader { get; set; } = "id";
    }

    public class DynamicTableRepository
    {
        private string connectionString;

        public DynamicTableRepository(string connString)
        {
            connectionString = connString;
        }

        public List<string> GetColumns(string tableName)
        {
            var columns = new List<string>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
                    "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME NOT IN ('id', 'created_at', 'updated_at') " +
                    "ORDER BY ORDINAL_POSITION", conn))
                {
                    cmd.Parameters.AddWithValue("@table", tableName);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            columns.Add(reader.GetString(0));
                    }
                }
            }
            return columns;
        }

        public DataTable GetAll(string tableName)
        {
            var dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"SELECT * FROM `{tableName}` ORDER BY id", conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable Search(string tableName, string searchText, List<string> columns)
        {
            if (columns == null || columns.Count == 0)
                return GetAll(tableName);

            var dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var conditions = new List<string>();
                foreach (var col in columns ?? new List<string>())
                    conditions.Add($"`{col}` LIKE @search");
                string query = $"SELECT * FROM `{tableName}` WHERE {string.Join(" OR ", conditions)} ORDER BY id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    using (var adapter = new MySqlDataAdapter(cmd))
                        adapter.Fill(dt);
                }
            }
            return dt;
        }

        public int Insert(string tableName, Dictionary<string, object> values)
        {
            var columns = new List<string>();
            var parameters = new List<string>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                foreach (var kv in values)
                {
                    columns.Add($"`{kv.Key}`");
                    parameters.Add($"@{kv.Key}");
                }
                string query = $"INSERT INTO `{tableName}` ({string.Join(",", columns)}) VALUES ({string.Join(",", parameters)}); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    foreach (var kv in values)
                        cmd.Parameters.AddWithValue($"@{kv.Key}", kv.Value ?? DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(string tableName, int id, Dictionary<string, object> values)
        {
            if (values == null || values.Count == 0)
                return;

            var setClauses = new List<string>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                foreach (var kv in values)
                    setClauses.Add($"`{kv.Key}` = @{kv.Key}");
                string query = $"UPDATE `{tableName}` SET {string.Join(",", setClauses)} WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    foreach (var kv in values)
                        cmd.Parameters.AddWithValue($"@{kv.Key}", kv.Value ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(string tableName, int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand($"DELETE FROM `{tableName}` WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateTable(string tableName)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = $@"
                    CREATE TABLE IF NOT EXISTS `{tableName}` (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(255) NOT NULL,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";
                using (var cmd = new MySqlCommand(sql, conn))
                    cmd.ExecuteNonQuery();
            }
        }

        public void CreateTableWithColumns(string tableName, List<ColumnDefinition> columns)
        {
            ValidateForeignKeyDefinitions(columns);

            var parts = new List<string>
            {
                "id INT AUTO_INCREMENT PRIMARY KEY"
            };
            var foreignKeys = new List<string>();

            foreach (var col in columns ?? new List<ColumnDefinition>())
            {
                parts.Add(BuildColumnSql(col));

                if (col.IsForeignKey && !string.IsNullOrEmpty(col.ReferenceTablesString))
                {
                    string cleanTable = GetSingleReferenceTable(col);
                    if (string.IsNullOrEmpty(cleanTable) || !TableExists(cleanTable)) continue;

                    string fkName = $"fk_{tableName}_{col.ColumnName}_{cleanTable}";
                    foreignKeys.Add($"CONSTRAINT `{fkName}` FOREIGN KEY (`{col.ColumnName}`) REFERENCES `{cleanTable}` (`{col.ReferenceHeader}`) ON DELETE CASCADE ON UPDATE CASCADE");
                }
            }

            parts.AddRange(foreignKeys);
            string sql = $"CREATE TABLE IF NOT EXISTS `{tableName}` ({string.Join(", ", parts)}) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"SQL Error: {ex.Message}\nQuery: {sql}");
                        throw;
                    }
                }
            }
        }

        public void SyncTableColumns(string tableName, List<ColumnDefinition> columns)
        {
            ValidateForeignKeyDefinitions(columns);

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                foreach (var col in columns ?? new List<ColumnDefinition>())
                {
                    bool exists = ColumnExists(conn, tableName, col.ColumnName);
                    if (!exists || !col.IsForeignKey)
                    {
                        string action = exists ? "MODIFY COLUMN" : "ADD COLUMN";
                        string sql = $"ALTER TABLE `{tableName}` {action} {BuildColumnSql(col)}";
                        using (var cmd = new MySqlCommand(sql, conn))
                            cmd.ExecuteNonQuery();
                    }

                    DropStaleForeignKeys(conn, tableName, col);
                    AddForeignKeys(conn, tableName, col);
                }
            }
        }

        private string BuildColumnSql(ColumnDefinition col)
        {
            string sqlType = col.IsForeignKey ? "INT" : col.DataType;
            string colDef = $"`{col.ColumnName}` {sqlType}";

            if (!col.IsForeignKey && col.Length.HasValue && (sqlType.StartsWith("VARCHAR") || sqlType.StartsWith("DECIMAL")))
                colDef += $"({col.Length.Value})";

            colDef += col.IsNullable ? " NULL" : " NOT NULL";

            if (!string.IsNullOrEmpty(col.Name))
                colDef += $" COMMENT '{col.Name.Replace("'", "\\'")}'";

            return colDef;
        }

        private void ValidateForeignKeyDefinitions(List<ColumnDefinition> columns)
        {
            foreach (var col in columns ?? new List<ColumnDefinition>())
            {
                if (col.IsForeignKey)
                    GetSingleReferenceTable(col);
            }
        }

        private void AddForeignKeys(MySqlConnection conn, string tableName, ColumnDefinition col)
        {
            if (!col.IsForeignKey || string.IsNullOrEmpty(col.ReferenceTablesString))
                return;

            string cleanTable = GetSingleReferenceTable(col);
            if (string.IsNullOrEmpty(cleanTable) || !TableExists(cleanTable)) return;

            string fkName = $"fk_{tableName}_{col.ColumnName}_{cleanTable}";
            if (ForeignKeyExists(conn, tableName, fkName)) return;

            string sql = $"ALTER TABLE `{tableName}` ADD CONSTRAINT `{fkName}` FOREIGN KEY (`{col.ColumnName}`) REFERENCES `{cleanTable}` (`{col.ReferenceHeader}`) ON DELETE CASCADE ON UPDATE CASCADE";
            using (var cmd = new MySqlCommand(sql, conn))
                cmd.ExecuteNonQuery();
        }

        private string GetSingleReferenceTable(ColumnDefinition col)
        {
            var tables = (col.ReferenceTablesString ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var cleanTables = new List<string>();

            foreach (var table in tables)
            {
                string cleanTable = table.Trim();
                if (!string.IsNullOrEmpty(cleanTable))
                    cleanTables.Add(cleanTable);
            }

            if (cleanTables.Count == 0)
                return null;

            if (cleanTables.Count > 1)
                throw new InvalidOperationException($"Колонка '{col.ColumnName}' не может ссылаться сразу на несколько таблиц. Создайте отдельную колонку для каждой связи.");

            return cleanTables[0];
        }

        private void DropStaleForeignKeys(MySqlConnection conn, string tableName, ColumnDefinition col)
        {
            string desiredTable = col.IsForeignKey ? GetSingleReferenceTable(col) : null;
            string desiredName = string.IsNullOrEmpty(desiredTable)
                ? null
                : $"fk_{tableName}_{col.ColumnName}_{desiredTable}";

            var constraints = new List<string>();
            string sql = @"SELECT CONSTRAINT_NAME
                           FROM information_schema.KEY_COLUMN_USAGE
                           WHERE CONSTRAINT_SCHEMA = DATABASE()
                           AND TABLE_NAME = @table
                           AND COLUMN_NAME = @column
                           AND REFERENCED_TABLE_NAME IS NOT NULL";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@column", col.ColumnName);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        constraints.Add(reader.GetString(0));
                }
            }

            foreach (var constraint in constraints)
            {
                if (constraint == desiredName)
                    continue;

                using (var cmd = new MySqlCommand($"ALTER TABLE `{tableName}` DROP FOREIGN KEY `{constraint}`", conn))
                    cmd.ExecuteNonQuery();
            }
        }

        private bool ColumnExists(MySqlConnection conn, string tableName, string columnName)
        {
            string sql = @"SELECT COUNT(*) FROM information_schema.COLUMNS 
                           WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = @column";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@column", columnName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool ForeignKeyExists(MySqlConnection conn, string tableName, string foreignKeyName)
        {
            string sql = @"SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS
                           WHERE CONSTRAINT_SCHEMA = DATABASE()
                           AND TABLE_NAME = @table
                           AND CONSTRAINT_NAME = @name
                           AND CONSTRAINT_TYPE = 'FOREIGN KEY'";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@name", foreignKeyName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool TableExists(string tableName)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM information_schema.tables WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@table", tableName);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public DataTable GetPage(string tableName, int page, int pageSize)
        {
            var dt = new DataTable();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = $"SELECT * FROM `{tableName}` ORDER BY id LIMIT @limit OFFSET @offset";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@limit", pageSize);
                    cmd.Parameters.AddWithValue("@offset", offset);
                    using (var adapter = new MySqlDataAdapter(cmd))
                        adapter.Fill(dt);
                }
            }
            return dt;
        }

        public int GetTotalCount(string tableName)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = $"SELECT COUNT(*) FROM `{tableName}`";
                using (var cmd = new MySqlCommand(sql, conn))
                    return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public DataTable SearchPage(string tableName, string searchText, int page, int pageSize, List<string> columns)
        {
            if (columns == null || columns.Count == 0)
                return GetPage(tableName, page, pageSize);

            var dt = new DataTable();
            int offset = (page - 1) * pageSize;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var conditions = new List<string>();
                foreach (var col in columns)
                    conditions.Add($"`{col}` LIKE @search");
                string whereClause = string.Join(" OR ", conditions);
                string sql = $"SELECT * FROM `{tableName}` WHERE {whereClause} ORDER BY id LIMIT @limit OFFSET @offset";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    cmd.Parameters.AddWithValue("@limit", pageSize);
                    cmd.Parameters.AddWithValue("@offset", offset);
                    using (var adapter = new MySqlDataAdapter(cmd))
                        adapter.Fill(dt);
                }
            }
            return dt;
        }

        public int GetSearchTotalCount(string tableName, string searchText, List<string> columns)
        {
            if (columns == null || columns.Count == 0)
                return GetTotalCount(tableName);

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var conditions = new List<string>();
                foreach (var col in columns)
                    conditions.Add($"`{col}` LIKE @search");
                string whereClause = string.Join(" OR ", conditions);
                string sql = $"SELECT COUNT(*) FROM `{tableName}` WHERE {whereClause}";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{searchText}%");
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
