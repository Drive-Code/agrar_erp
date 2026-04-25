using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class AddDirectoryForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblName, lblTableName, lblDescription, lblColumnCount;
        private TextBox txtName, txtTableName, txtDescription;
        private NumericUpDown nudColumnCount;
        private Button btnGenerateGrid, btnAddRow, btnRemoveRow, btnSelectIcon;
        private DataGridView dgvColumns;
        private Button btnSave, btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // AddDirectoryForm
            this.ClientSize = new Size(980, 720);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(32, 32, 40);
            this.ForeColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 11);
            this.Name = "AddDirectoryForm";
            this.Text = "Создание нового справочника";

            // ========== ОСНОВНЫЕ ПОЛЯ ==========
            this.lblName = new Label();
            this.lblName.Text = "Название справочника:";
            this.lblName.Location = new Point(20, 20);
            this.lblName.AutoSize = true;
            this.Controls.Add(this.lblName);

            this.txtName = new TextBox();
            this.txtName.Location = new Point(250, 17);
            this.txtName.Size = new Size(300, 30);
            this.txtName.BackColor = Color.FromArgb(50, 50, 60);
            this.txtName.ForeColor = Color.WhiteSmoke;
            this.txtName.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.txtName);

            // Кнопка выбора иконки
            this.btnSelectIcon = new Button();
            this.btnSelectIcon.Text = "Выбрать иконку";
            this.btnSelectIcon.Size = new Size(180, 30);
            this.btnSelectIcon.Location = new Point(570, 17);
            this.btnSelectIcon.BackColor = Color.FromArgb(70, 130, 180);
            this.btnSelectIcon.ForeColor = Color.White;
            this.btnSelectIcon.FlatStyle = FlatStyle.Flat;
            this.btnSelectIcon.FlatAppearance.BorderSize = 0;
            this.btnSelectIcon.Click += new System.EventHandler(this.BtnSelectIcon_Click);
            this.Controls.Add(this.btnSelectIcon);

            this.lblTableName = new Label();
            this.lblTableName.Text = "Имя таблицы (англ):";
            this.lblTableName.Location = new Point(20, 60);
            this.lblTableName.AutoSize = true;
            this.Controls.Add(this.lblTableName);

            this.txtTableName = new TextBox();
            this.txtTableName.Location = new Point(250, 57);
            this.txtTableName.Size = new Size(160, 30);
            this.txtTableName.BackColor = Color.FromArgb(50, 50, 60);
            this.txtTableName.ForeColor = Color.WhiteSmoke;
            this.txtTableName.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.txtTableName);

            this.lblColumnCount = new Label();
            this.lblColumnCount.Text = "Количество колонок:";
            this.lblColumnCount.Location = new Point(430, 60);
            this.lblColumnCount.AutoSize = true;
            this.Controls.Add(this.lblColumnCount);

            this.nudColumnCount = new NumericUpDown();
            this.nudColumnCount.Location = new Point(600, 58);
            this.nudColumnCount.Size = new Size(80, 30);
            this.nudColumnCount.Minimum = 1;
            this.nudColumnCount.Maximum = 50;
            this.nudColumnCount.Value = 2;
            this.nudColumnCount.BackColor = Color.FromArgb(50, 50, 60);
            this.nudColumnCount.ForeColor = Color.WhiteSmoke;
            this.Controls.Add(this.nudColumnCount);

            this.btnGenerateGrid = new Button();
            this.btnGenerateGrid.Text = "Создать сетку";
            this.btnGenerateGrid.Size = new Size(180, 30);
            this.btnGenerateGrid.Location = new Point(700, 58);
            this.btnGenerateGrid.BackColor = Color.FromArgb(70, 130, 180);
            this.btnGenerateGrid.ForeColor = Color.White;
            this.btnGenerateGrid.FlatStyle = FlatStyle.Flat;
            this.btnGenerateGrid.FlatAppearance.BorderSize = 0;
            this.btnGenerateGrid.Click += new System.EventHandler(this.BtnGenerateGrid_Click);
            this.Controls.Add(this.btnGenerateGrid);

            this.lblDescription = new Label();
            this.lblDescription.Text = "Описание:";
            this.lblDescription.Location = new Point(20, 100);
            this.lblDescription.AutoSize = true;
            this.Controls.Add(this.lblDescription);

            this.txtDescription = new TextBox();
            this.txtDescription.Location = new Point(250, 97);
            this.txtDescription.Size = new Size(690, 60);
            this.txtDescription.Multiline = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.BackColor = Color.FromArgb(50, 50, 60);
            this.txtDescription.ForeColor = Color.WhiteSmoke;
            this.txtDescription.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.txtDescription);

            // ========== СЕТКА КОЛОНОК ==========
            this.dgvColumns = new DataGridView();
            this.dgvColumns.Location = new Point(20, 180);
            this.dgvColumns.Size = new Size(940, 420);
            this.dgvColumns.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvColumns.BackgroundColor = Color.FromArgb(40, 40, 50);
            this.dgvColumns.BorderStyle = BorderStyle.None;
            this.dgvColumns.AllowUserToAddRows = false;
            this.dgvColumns.RowHeadersVisible = false;
            this.Controls.Add(this.dgvColumns);

            this.btnAddRow = new Button();
            this.btnAddRow.Text = "+ Добавить колонку";
            this.btnAddRow.Size = new Size(180, 35);
            this.btnAddRow.Location = new Point(20, 610);
            this.btnAddRow.BackColor = Color.FromArgb(70, 130, 180);
            this.btnAddRow.ForeColor = Color.White;
            this.btnAddRow.FlatStyle = FlatStyle.Flat;
            this.btnAddRow.FlatAppearance.BorderSize = 0;
            this.btnAddRow.Click += new System.EventHandler(this.BtnAddRow_Click);
            this.Controls.Add(this.btnAddRow);

            this.btnRemoveRow = new Button();
            this.btnRemoveRow.Text = "- Удалить колонку";
            this.btnRemoveRow.Size = new Size(180, 35);
            this.btnRemoveRow.Location = new Point(210, 610);
            this.btnRemoveRow.BackColor = Color.FromArgb(180, 60, 60);
            this.btnRemoveRow.ForeColor = Color.White;
            this.btnRemoveRow.FlatStyle = FlatStyle.Flat;
            this.btnRemoveRow.FlatAppearance.BorderSize = 0;
            this.btnRemoveRow.Click += new System.EventHandler(this.BtnRemoveRow_Click);
            this.Controls.Add(this.btnRemoveRow);

            this.btnSave = new Button();
            this.btnSave.Text = "Создать справочник";
            this.btnSave.Size = new Size(220, 40);
            this.btnSave.Location = new Point(450, 665);
            this.btnSave.BackColor = Color.FromArgb(70, 130, 180);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            this.Controls.Add(this.btnSave);

            this.btnCancel = new Button();
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Size = new Size(120, 40);
            this.btnCancel.Location = new Point(690, 665);
            this.btnCancel.BackColor = Color.FromArgb(100, 100, 120);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            this.Controls.Add(this.btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}