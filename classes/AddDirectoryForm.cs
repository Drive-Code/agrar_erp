using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Repositories;

namespace WindowsFormsApp1
{
    public partial class AddDirectoryForm : Form
    {
        private string connectionString;
        private List<string> existingTables = new List<string>();
        private int? editDirectoryId = null;
        private bool isEditMode = false;
        private string selectedIcon = "Book";
        private bool isMultiSelectOpen = false;
        private Dictionary<int, string> rowSelections = new Dictionary<int, string>();

        public AddDirectoryForm(string connString)
        {
            connectionString = connString;
            isEditMode = false;
            InitializeComponent();
            LoadExistingTables();
            this.Text = "Создание нового справочника";
        }

        public AddDirectoryForm(string connString, int id)
        {
            connectionString = connString;
            editDirectoryId = id;
            isEditMode = true;
            InitializeComponent();
            LoadExistingTables();
            this.Text = "Редактирование справочника";
            LoadExistingDirectory();
        }

        private void LoadExistingTables()
        {
            try
            {
                using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new MySqlConnector.MySqlCommand(
                        "SELECT TABLE_NAME FROM information_schema.tables WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME NOT IN ('directories', 'users') ORDER BY TABLE_NAME", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            existingTables.Add(reader.GetString(0));
                    }
                }
            }
            catch { }
        }

        private void LoadExistingDirectory()
        {
            try
            {
                var dirRepo = new DirectoryRepository(connectionString);
                var item = dirRepo.GetById(editDirectoryId.Value);
                if (item != null)
                {
                    txtName.Text = item.Name;
                    txtTableName.Text = item.TableName;
                    txtTableName.ReadOnly = true;
                    txtDescription.Text = item.Description ?? "";
                    selectedIcon = item.IconName ?? "Book";
                    btnSelectIcon.Text = selectedIcon;

                    var dynRepo = new DynamicTableRepository(connectionString);
                    var columns = dynRepo.GetColumns(item.TableName);
                    nudColumnCount.Value = Math.Max(columns.Count, 1);
                    BtnGenerateGrid_Click(null, null);

                    // Загружаем данные колонок из INFORMATION_SCHEMA
                    using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string sql = @"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE, COLUMN_COMMENT
                                       FROM INFORMATION_SCHEMA.COLUMNS
                                       WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table 
                                       AND COLUMN_NAME NOT IN ('id', 'created_at', 'updated_at')
                                       ORDER BY ORDINAL_POSITION";
                        using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@table", item.TableName);
                            using (var reader = cmd.ExecuteReader())
                            {
                                int i = 0;
                                while (reader.Read() && i < dgvColumns.Rows.Count)
                                {
                                    var row = dgvColumns.Rows[i];
                                    string colName = reader.GetString(0);
                                    string comment = reader.IsDBNull(4) ? "" : reader.GetString(4);

                                    row.Cells["ColumnName"].Value = colName;
                                    row.Cells["Name"].Value = string.IsNullOrEmpty(comment) ? colName : comment;

                                    string dataType = reader.GetString(1).ToUpper();
                                    if (dataType.StartsWith("VARCHAR")) dataType = "VARCHAR";
                                    else if (dataType.StartsWith("DECIMAL")) dataType = "DECIMAL";
                                    else if (dataType == "DATE") dataType = "DATE";
                                    else if (dataType == "TEXT") dataType = "TEXT";
                                    else dataType = "INT";
                                    row.Cells["DataType"].Value = dataType;

                                    int? len = reader.IsDBNull(2) ? (int?)null : Convert.ToInt32(reader.GetValue(2));
                                    row.Cells["Length"].Value = len ?? 255;
                                    row.Cells["IsNullable"].Value = reader.GetString(3) == "YES";

                                    // Проверяем внешние ключи
                                    var refTables = GetForeignKeyReferences(item.TableName, colName);
                                    if (refTables.Count > 0)
                                    {
                                        row.Cells["IsForeignKey"].Value = true;
                                        string refTablesStr = string.Join(",", refTables);
                                        rowSelections[i] = refTablesStr;
                                        row.Cells["ReferenceTables"].Value = refTablesStr;
                                    }
                                    else
                                    {
                                        row.Cells["IsForeignKey"].Value = false;
                                    }

                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных справочника:\n" + ex.Message);
            }
        }

        private List<string> GetForeignKeyReferences(string tableName, string columnName)
        {
            var refTables = new List<string>();
            try
            {
                using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT REFERENCED_TABLE_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                                   WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table 
                                   AND COLUMN_NAME = @column AND REFERENCED_TABLE_NAME IS NOT NULL";
                    using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@table", tableName);
                        cmd.Parameters.AddWithValue("@column", columnName);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                refTables.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch { }
            return refTables;
        }

        private void BtnSelectIcon_Click(object sender, EventArgs e)
        {
            using (var form = new IconPickerForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    selectedIcon = form.SelectedIconName;
                    btnSelectIcon.Text = selectedIcon;
                }
            }
        }

        private void BtnGenerateGrid_Click(object sender, EventArgs e)
        {
            int count = (int)nudColumnCount.Value;
            dgvColumns.Columns.Clear();
            dgvColumns.Rows.Clear();
            rowSelections.Clear();

            DataGridViewTextBoxColumn nameCol = new DataGridViewTextBoxColumn();
            nameCol.HeaderText = "Название (рус)";
            nameCol.Name = "Name";
            nameCol.DefaultCellStyle.ForeColor = Color.Black;
            dgvColumns.Columns.Add(nameCol);

            DataGridViewTextBoxColumn colNameCol = new DataGridViewTextBoxColumn();
            colNameCol.HeaderText = "Имя (англ)";
            colNameCol.Name = "ColumnName";
            colNameCol.DefaultCellStyle.ForeColor = Color.Black;
            dgvColumns.Columns.Add(colNameCol);

            DataGridViewComboBoxColumn typeCol = new DataGridViewComboBoxColumn();
            typeCol.HeaderText = "Тип данных";
            typeCol.Name = "DataType";
            typeCol.Items.AddRange("INT", "VARCHAR", "DECIMAL", "DATE", "TEXT");
            typeCol.DefaultCellStyle.ForeColor = Color.Black;
            dgvColumns.Columns.Add(typeCol);

            DataGridViewTextBoxColumn lengthCol = new DataGridViewTextBoxColumn();
            lengthCol.HeaderText = "Длина";
            lengthCol.Name = "Length";
            lengthCol.DefaultCellStyle.ForeColor = Color.Black;
            dgvColumns.Columns.Add(lengthCol);

            DataGridViewCheckBoxColumn nullableCol = new DataGridViewCheckBoxColumn();
            nullableCol.HeaderText = "NULL";
            nullableCol.Name = "IsNullable";
            dgvColumns.Columns.Add(nullableCol);

            DataGridViewCheckBoxColumn fkCol = new DataGridViewCheckBoxColumn();
            fkCol.HeaderText = "Внешний ключ";
            fkCol.Name = "IsForeignKey";
            dgvColumns.Columns.Add(fkCol);

            DataGridViewButtonColumn refBtnCol = new DataGridViewButtonColumn();
            refBtnCol.HeaderText = "Связи (выберите таблицы)";
            refBtnCol.Name = "ReferenceTables";
            refBtnCol.Text = "...";
            refBtnCol.UseColumnTextForButtonValue = true;
            refBtnCol.DefaultCellStyle.ForeColor = Color.Black;
            dgvColumns.Columns.Add(refBtnCol);

            dgvColumns.CellContentClick += DgvColumns_CellContentClick;
            dgvColumns.DefaultCellStyle.ForeColor = Color.Black;

            for (int i = 0; i < count; i++)
                dgvColumns.Rows.Add($"Колонка {i + 1}", $"column_{i + 1}", "VARCHAR", 255, true, false, "");
        }

        private void DgvColumns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (isMultiSelectOpen) return;

            if (dgvColumns.Columns[e.ColumnIndex].Name == "ReferenceTables")
            {
                isMultiSelectOpen = true;
                var row = dgvColumns.Rows[e.RowIndex];

                string current = rowSelections.ContainsKey(e.RowIndex)
                    ? rowSelections[e.RowIndex]
                    : row.Cells["ReferenceTables"].Value?.ToString() ?? "";

                using (var form = new MultiSelectTableForm(existingTables, current))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        rowSelections[e.RowIndex] = form.SelectedTables;
                        row.Cells["ReferenceTables"].Value = form.SelectedTables;
                    }
                }
                isMultiSelectOpen = false;
            }
        }

        private void BtnAddRow_Click(object sender, EventArgs e)
        {
            dgvColumns.Rows.Add("Новая колонка", "new_column", "VARCHAR", 255, true, false, "");
        }

        private void BtnRemoveRow_Click(object sender, EventArgs e)
        {
            if (dgvColumns.SelectedRows.Count > 0)
            {
                int index = dgvColumns.SelectedRows[0].Index;
                if (rowSelections.ContainsKey(index))
                    rowSelections.Remove(index);
                dgvColumns.Rows.RemoveAt(index);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название справочника.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTableName.Text))
            {
                MessageBox.Show("Введите имя таблицы (англ).");
                return;
            }

            string tableName = txtTableName.Text.Trim().Replace(" ", "_").ToLower();

            try
            {
                // Собираем определения колонок
                var columns = new List<ColumnDefinition>();
                foreach (DataGridViewRow row in dgvColumns.Rows)
                {
                    if (row.IsNewRow) continue;
                    bool isFK = (bool)(row.Cells["IsForeignKey"].Value ?? false);
                    int rowIndex = row.Index;

                    string refTables = rowSelections.ContainsKey(rowIndex)
                        ? rowSelections[rowIndex]
                        : row.Cells["ReferenceTables"].Value?.ToString() ?? "";

                    string russianName = row.Cells["Name"].Value?.ToString() ?? "";
                    string engName = row.Cells["ColumnName"].Value?.ToString()?.Replace(" ", "_").ToLower() ?? "";

                    columns.Add(new ColumnDefinition
                    {
                        Name = russianName,
                        ColumnName = engName,
                        DataType = isFK ? "INT" : (row.Cells["DataType"].Value?.ToString() ?? "VARCHAR"),
                        Length = isFK ? null : (int.TryParse(row.Cells["Length"].Value?.ToString(), out int len) ? len : (int?)null),
                        IsNullable = isFK ? true : (bool)(row.Cells["IsNullable"].Value ?? true),
                        IsForeignKey = isFK,
                        ReferenceTablesString = refTables,
                        ReferenceHeader = "id"
                    });
                }

                if (isEditMode)
                {
                    // Обновляем запись в directories
                    var dirRepo = new DirectoryRepository(connectionString);
                    dirRepo.Update(new DirectoryItem
                    {
                        Id = editDirectoryId.Value,
                        Name = txtName.Text.Trim(),
                        Description = txtDescription.Text.Trim(),
                        IconName = selectedIcon,
                        TableName = tableName
                    });

                    MessageBox.Show("Справочник обновлён!");
                }
                else
                {
                    // Создаём запись в directories
                    var dirRepo = new DirectoryRepository(connectionString);
                    dirRepo.Insert(new DirectoryItem
                    {
                        Name = txtName.Text.Trim(),
                        Description = txtDescription.Text.Trim(),
                        IconName = selectedIcon,
                        TableName = tableName
                    });

                    // Создаём таблицу
                    var dynRepo = new DynamicTableRepository(connectionString);
                    dynRepo.CreateTableWithColumns(tableName, columns);

                    // ТЕПЕРЬ ГЛАВНОЕ: для КАЖДОЙ колонки-связи добавляем её в указанные таблицы
                    foreach (var col in columns)
                    {
                        if (col.IsForeignKey && !string.IsNullOrEmpty(col.ReferenceTablesString))
                        {
                            var targetTables = col.ReferenceTablesString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var targetTable in targetTables)
                            {
                                string cleanTarget = targetTable.Trim();
                                if (cleanTarget == tableName) continue; // пропускаем себя

                                // Добавляем колонку `tableName`_id в целевую таблицу
                                string newColName = $"{tableName}_id";

                                if (!ColumnExists(cleanTarget, newColName))
                                {
                                    AddColumnWithForeignKey(cleanTarget, newColName, tableName, col.Name);
                                }
                            }
                        }
                    }

                    MessageBox.Show($"Справочник '{txtName.Text}' создан!\nТаблица: {tableName}");
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания справочника:\n" + ex.Message);
            }
        }

        private bool ColumnExists(string tableName, string columnName)
        {
            try
            {
                using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT COUNT(*) FROM information_schema.COLUMNS 
                           WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = @column";
                    using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@table", tableName);
                        cmd.Parameters.AddWithValue("@column", columnName);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch { return false; }
        }

        private void AddColumnWithForeignKey(string targetTable, string newColName, string referenceTable, string russianName)
        {
            try
            {
                using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Добавляем колонку с комментарием (русским названием) и внешним ключом
                    string sql = $@"ALTER TABLE `{targetTable}` 
                            ADD COLUMN `{newColName}` INT NULL COMMENT '{russianName.Replace("'", "\\'")}',
                            ADD CONSTRAINT `fk_{targetTable}_{newColName}` 
                            FOREIGN KEY (`{newColName}`) REFERENCES `{referenceTable}` (`id`) 
                            ON DELETE SET NULL ON UPDATE CASCADE";

                    using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Если колонка уже есть — игнорируем
                if (!ex.Message.Contains("Duplicate"))
                    System.Diagnostics.Debug.WriteLine($"Error adding column {newColName} to {targetTable}: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}