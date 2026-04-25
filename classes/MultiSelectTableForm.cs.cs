using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class MultiSelectTableForm : Form
    {
        private CheckedListBox clbTables;
        private Button btnOK, btnCancel;
        private string currentSelection;

        public string SelectedTables { get; private set; }

        public MultiSelectTableForm(List<string> tables, string currentSelection)
        {
            this.currentSelection = currentSelection;

            this.Text = "Выберите таблицы для связи";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(32, 32, 40);
            this.ForeColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 11);

            Label lbl = new Label
            {
                Text = "Отметьте нужные таблицы:",
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.WhiteSmoke
            };
            this.Controls.Add(lbl);

            clbTables = new CheckedListBox
            {
                Location = new Point(20, 50),
                Size = new Size(340, 250),
                BackColor = Color.FromArgb(50, 50, 60),
                ForeColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 11)
            };
            this.Controls.Add(clbTables);

            btnOK = new Button
            {
                Text = "OK",
                Size = new Size(100, 35),
                Location = new Point(120, 310),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.Click += BtnOK_Click;
            this.Controls.Add(btnOK);

            btnCancel = new Button
            {
                Text = "Отмена",
                Size = new Size(100, 35),
                Location = new Point(240, 310),
                BackColor = Color.FromArgb(100, 100, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);

            LoadTables(tables);
        }

        private void LoadTables(List<string> tables)
        {
            var selected = currentSelection.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(s => s.Trim())
                                           .ToList();

            foreach (var table in tables)
            {
                bool isChecked = selected.Contains(table.Trim());
                clbTables.Items.Add(table.Trim(), isChecked);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            SelectedTables = string.Join(",", clbTables.CheckedItems.Cast<string>());
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}