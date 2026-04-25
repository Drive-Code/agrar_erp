using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Repositories;

namespace WindowsFormsApp1
{
    public partial class AddEditItemForm : Form
    {
        private string tableName;
        private int? itemId;
        private string connectionString;
        private DynamicTableRepository dynRepo;
        private Dictionary<string, Control> fieldControls = new Dictionary<string, Control>();

        public AddEditItemForm(string connString, string table, int? id = null)
        {
            connectionString = connString;
            tableName = table;
            itemId = id;
            dynRepo = new DynamicTableRepository(connectionString);
            InitializeComponent();
            LoadForm();
            if (itemId.HasValue)
                LoadExistingData();
        }

        private string GetColumnComment(string colName)
        {
            try
            {
                using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS 
                                   WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = @column";
                    using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@table", tableName);
                        cmd.Parameters.AddWithValue("@column", colName);
                        var result = cmd.ExecuteScalar();
                        string comment = result?.ToString() ?? "";
                        return string.IsNullOrEmpty(comment) ? colName.Replace("_", " ") : comment;
                    }
                }
            }
            catch { return colName.Replace("_", " "); }
        }

        private string GetReferencedTable(string colName)
        {
            try
            {
                using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT REFERENCED_TABLE_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                                   WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table 
                                   AND COLUMN_NAME = @column AND REFERENCED_TABLE_NAME IS NOT NULL LIMIT 1";
                    using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@table", tableName);
                        cmd.Parameters.AddWithValue("@column", colName);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch { return null; }
        }

        private void LoadForm()
        {
            this.Text = itemId == null ? $"Добавление в {tableName}" : $"Редактирование в {tableName}";
            var columns = dynRepo.GetColumns(tableName);

            int y = 15;
            int labelX = 15;
            int textX = 220;
            int textWidth = 360;

            foreach (var col in columns)
            {
                // Получаем красивое название из COMMENT
                string displayName = GetColumnComment(col);

                // Если название заканчивается на _id, обрезаем для отображения
                if (col.EndsWith("_id", StringComparison.OrdinalIgnoreCase) && displayName == col.Replace("_", " "))
                {
                    displayName = col.Substring(0, col.Length - 3).Replace("_", " ");
                }

                var lbl = new Label
                {
                    Text = displayName + ":",
                    Location = new Point(labelX, y),
                    AutoSize = true,
                    ForeColor = Color.WhiteSmoke,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold)
                };
                dynamicPanel.Controls.Add(lbl);

                Control ctrl;

                // Проверяем, является ли колонка внешним ключом
                string refTable = GetReferencedTable(col);

                if (refTable != null)
                {
                    // Это внешний ключ — показываем ВЫПАДАЮЩИЙ СПИСОК со значениями из связанной таблицы
                    var cmb = new ComboBox
                    {
                        Location = new Point(textX, y - 3),
                        Size = new Size(textWidth, 32),
                        BackColor = Color.FromArgb(50, 50, 60),
                        ForeColor = Color.WhiteSmoke,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Tag = col
                    };

                    LoadComboBoxData(cmb, refTable);
                    ctrl = cmb;
                }
                else if (col.IndexOf("date", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var dtp = new DateTimePicker
                    {
                        Location = new Point(textX, y - 3),
                        Size = new Size(textWidth, 32),
                        Format = DateTimePickerFormat.Short,
                        ShowCheckBox = true,
                        Checked = false,
                        Tag = col
                    };
                    ctrl = dtp;
                }
                else
                {
                    var txt = new TextBox
                    {
                        Location = new Point(textX, y - 3),
                        Size = new Size(textWidth, 32),
                        BackColor = Color.FromArgb(50, 50, 60),
                        ForeColor = Color.WhiteSmoke,
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = col
                    };
                    ctrl = txt;
                }

                dynamicPanel.Controls.Add(ctrl);
                fieldControls[col] = ctrl;
                y += 55;
            }

            // Рассчитываем позицию кнопок
            int buttonY = y + 30;

            // Перемещаем кнопки в правильную позицию
            btnSave.Text = itemId == null ? "Добавить" : "Сохранить изменения";
            btnSave.Location = new Point(120, buttonY);
            btnSave.Size = new Size(200, 50);
            btnSave.Visible = true;
            btnSave.BringToFront();

            btnCancel.Text = "Отмена";
            btnCancel.Location = new Point(340, buttonY);
            btnCancel.Size = new Size(160, 50);
            btnCancel.Visible = true;
            btnCancel.BringToFront();

            // Увеличиваем размер формы чтобы всё поместилось
            this.ClientSize = new Size(620, buttonY + 100);

            // Обновляем панель
            dynamicPanel.Height = buttonY + 50;
        }

        private void LoadComboBoxData(ComboBox cmb, string refTable)
        {
            try
            {
                using (var conn = new MySqlConnector.MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Проверяем наличие колонки name
                    bool hasNameColumn = false;
                    string checkSql = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                                        WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = 'name'";
                    using (var cmd = new MySqlConnector.MySqlCommand(checkSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@table", refTable);
                        hasNameColumn = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }

                    string displayCol = hasNameColumn ? "name" : "id";
                    string sql = $"SELECT id, `{displayCol}` FROM `{refTable}` ORDER BY `{displayCol}`";

                    using (var cmd = new MySqlConnector.MySqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        cmb.Items.Clear();
                        cmb.Items.Add(new ComboBoxItem { Value = "", Display = "(не выбрано)" });

                        while (reader.Read())
                        {
                            cmb.Items.Add(new ComboBoxItem
                            {
                                Value = reader[0].ToString(),
                                Display = reader[1].ToString()
                            });
                        }
                    }

                    if (cmb.Items.Count > 0)
                        cmb.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading combo for {refTable}: {ex.Message}");
            }
        }

        private void LoadExistingData()
        {
            try
            {
                var dataTable = dynRepo.GetAll(tableName);
                var rows = dataTable.Select($"id = {itemId}");
                if (rows.Length > 0)
                {
                    var row = rows[0];
                    foreach (var col in fieldControls.Keys)
                    {
                        if (row[col] == DBNull.Value) continue;

                        if (fieldControls[col] is DateTimePicker dtp)
                        {
                            object val = row[col];
                            if (val is DateTime dateValue && dateValue != DateTime.MinValue)
                            {
                                dtp.Checked = true;
                                dtp.Value = dateValue;
                            }
                            else
                            {
                                dtp.Checked = false;
                            }
                        }
                        else if (fieldControls[col] is ComboBox cmb)
                        {
                            string val = row[col].ToString();
                            bool found = false;
                            foreach (ComboBoxItem item in cmb.Items)
                            {
                                if (item.Value == val)
                                {
                                    cmb.SelectedItem = item;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found && cmb.Items.Count > 0)
                                cmb.SelectedIndex = 0;
                        }
                        else if (fieldControls[col] is TextBox txt)
                        {
                            txt.Text = row[col].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных:\n" + ex.Message);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var values = new Dictionary<string, object>();
                foreach (var kv in fieldControls)
                {
                    if (kv.Value is DateTimePicker dtp)
                    {
                        values[kv.Key] = dtp.Checked && dtp.Value != DateTime.MinValue
                            ? (object)dtp.Value
                            : DBNull.Value;
                    }
                    else if (kv.Value is ComboBox cmb)
                    {
                        if (cmb.SelectedItem is ComboBoxItem item && !string.IsNullOrEmpty(item.Value))
                            values[kv.Key] = Convert.ToInt32(item.Value);
                        else
                            values[kv.Key] = DBNull.Value;
                    }
                    else if (kv.Value is TextBox txt)
                    {
                        string val = txt.Text.Trim();
                        if (string.IsNullOrEmpty(val))
                            values[kv.Key] = DBNull.Value;
                        else
                            values[kv.Key] = val;
                    }
                }

                if (itemId.HasValue)
                    dynRepo.Update(tableName, itemId.Value, values);
                else
                    dynRepo.Insert(tableName, values);

                MessageBox.Show("Сохранено успешно!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения:\n" + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    public class ComboBoxItem
    {
        public string Value { get; set; }
        public string Display { get; set; }
        public override string ToString() => Display;
    }
}