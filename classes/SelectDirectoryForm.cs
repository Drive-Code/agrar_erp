using MySqlConnector;
using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Repositories;

namespace WindowsFormsApp1
{
    public class SelectDirectoryForm : Form
    {
        private string connectionString;
        private ComboBox cmbDirectories;
        private Button btnOK;
        private Button btnCancel;
        private DirectoryRepository dirRepo;

        public int? SelectedDirectoryId { get; private set; }
        public string SelectedDirectoryName { get; private set; }

        public SelectDirectoryForm(string connString, string title)
        {
            connectionString = connString;
            dirRepo = new DirectoryRepository(connectionString);
            this.Text = title;
            this.Size = new Size(400, 180);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lbl = new Label
            {
                Text = "Выберите справочник:",
                Location = new Point(20, 20),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 11)
            };
            this.Controls.Add(lbl);

            cmbDirectories = new ComboBox
            {
                Location = new Point(20, 55),
                Size = new Size(345, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11)
            };
            this.Controls.Add(cmbDirectories);

            btnOK = new Button
            {
                Text = "OK",
                Location = new Point(180, 100),
                Size = new Size(85, 35),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;
            this.Controls.Add(btnOK);

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(280, 100),
                Size = new Size(85, 35),
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancel);

            LoadDirectories();
        }

        private void LoadDirectories()
        {
            try
            {
                var dirs = dirRepo.GetAll();
                foreach (var d in dirs)
                {
                    cmbDirectories.Items.Add(new DirectoryItem { Id = d.Id, Name = d.Name });
                }
                if (cmbDirectories.Items.Count > 0)
                    cmbDirectories.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (cmbDirectories.SelectedItem is DirectoryItem item)
            {
                SelectedDirectoryId = item.Id;
                SelectedDirectoryName = item.Name;
            }
            else
            {
                MessageBox.Show("Выберите справочник");
                this.DialogResult = DialogResult.None;
            }
        }

        private class DirectoryItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public override string ToString() => Name;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "SelectDirectoryForm";
            this.Load += new System.EventHandler(this.SelectDirectoryForm_Load);
            this.ResumeLayout(false);

        }

        private void SelectDirectoryForm_Load(object sender, EventArgs e)
        {

        }
    }
}