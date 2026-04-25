// AddEditMaterialForm.Designer.cs
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class AddEditMaterialForm : Form
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblName, lblBatch, lblMass, lblLength, lblQuantity, lblArrival, lblConsumption, lblArrivalDate;
        private TextBox txtName, txtBatch, txtMass, txtLength, txtQuantity, txtConsumption;
        private ComboBox cmbArrival;
        private DateTimePicker dtpArrivalDate;
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

            this.Size = new Size(550, 640);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(32, 32, 40);
            this.ForeColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 11);
            this.Name = "AddEditMaterialForm";

            this.lblName = new Label();
            this.lblName.Text = "Название:";
            this.lblName.Location = new Point(20, 30);
            this.lblName.AutoSize = true;
            this.Controls.Add(this.lblName);

            this.txtName = new TextBox();
            this.txtName.Location = new Point(160, 27);
            this.txtName.Size = new Size(370, 30);
            this.txtName.BackColor = Color.FromArgb(50, 50, 60);
            this.txtName.ForeColor = Color.WhiteSmoke;
            this.Controls.Add(this.txtName);

            this.lblBatch = new Label();
            this.lblBatch.Text = "Партия:";
            this.lblBatch.Location = new Point(20, 80);
            this.lblBatch.AutoSize = true;
            this.Controls.Add(this.lblBatch);

            this.txtBatch = new TextBox();
            this.txtBatch.Location = new Point(160, 77);
            this.txtBatch.Size = new Size(370, 30);
            this.txtBatch.BackColor = Color.FromArgb(50, 50, 60);
            this.txtBatch.ForeColor = Color.WhiteSmoke;
            this.Controls.Add(this.txtBatch);

            this.lblMass = new Label();
            this.lblMass.Text = "Полная масса (кг):";
            this.lblMass.Location = new Point(20, 125);
            this.lblMass.AutoSize = true;
            this.Controls.Add(this.lblMass);

            this.txtMass = new TextBox();
            this.txtMass.Location = new Point(160, 122);
            this.txtMass.Size = new Size(370, 30);
            this.txtMass.BackColor = Color.FromArgb(50, 50, 60);
            this.txtMass.ForeColor = Color.WhiteSmoke;
            this.Controls.Add(this.txtMass);

            this.lblLength = new Label();
            this.lblLength.Text = "Длина (м):";
            this.lblLength.Location = new Point(20, 170);
            this.lblLength.AutoSize = true;
            this.Controls.Add(this.lblLength);

            this.txtLength = new TextBox();
            this.txtLength.Location = new Point(160, 167);
            this.txtLength.Size = new Size(370, 30);
            this.txtLength.BackColor = Color.FromArgb(50, 50, 60);
            this.txtLength.ForeColor = Color.WhiteSmoke;
            this.Controls.Add(this.txtLength);

            this.lblQuantity = new Label();
            this.lblQuantity.Text = "Количество:";
            this.lblQuantity.Location = new Point(20, 215);
            this.lblQuantity.AutoSize = true;
            this.Controls.Add(this.lblQuantity);

            this.txtQuantity = new TextBox();
            this.txtQuantity.Location = new Point(160, 212);
            this.txtQuantity.Size = new Size(370, 30);
            this.txtQuantity.BackColor = Color.FromArgb(50, 50, 60);
            this.txtQuantity.ForeColor = Color.WhiteSmoke;
            this.Controls.Add(this.txtQuantity);

            this.lblArrival = new Label();
            this.lblArrival.Text = "Поставщик:";
            this.lblArrival.Location = new Point(20, 260);
            this.lblArrival.AutoSize = true;
            this.Controls.Add(this.lblArrival);

            this.cmbArrival = new ComboBox();
            this.cmbArrival.Location = new Point(160, 257);
            this.cmbArrival.Size = new Size(370, 30);
            this.cmbArrival.BackColor = Color.FromArgb(50, 50, 60);
            this.cmbArrival.ForeColor = Color.WhiteSmoke;
            this.cmbArrival.DropDownStyle = ComboBoxStyle.DropDown;
            this.Controls.Add(this.cmbArrival);

            this.lblConsumption = new Label();
            this.lblConsumption.Text = "Расход в:";
            this.lblConsumption.Location = new Point(20, 305);
            this.lblConsumption.AutoSize = true;
            this.Controls.Add(this.lblConsumption);

            this.txtConsumption = new TextBox();
            this.txtConsumption.Location = new Point(160, 302);
            this.txtConsumption.Size = new Size(370, 30);
            this.txtConsumption.BackColor = Color.FromArgb(50, 50, 60);
            this.txtConsumption.ForeColor = Color.WhiteSmoke;
            this.Controls.Add(this.txtConsumption);

            this.lblArrivalDate = new Label();
            this.lblArrivalDate.Text = "Дата поступления:";
            this.lblArrivalDate.Location = new Point(20, 350);
            this.lblArrivalDate.AutoSize = true;
            this.Controls.Add(this.lblArrivalDate);

            this.dtpArrivalDate = new DateTimePicker();
            this.dtpArrivalDate.Location = new Point(160, 347);
            this.dtpArrivalDate.Size = new Size(370, 30);
            this.dtpArrivalDate.Format = DateTimePickerFormat.Short;
            this.dtpArrivalDate.ShowCheckBox = true;
            this.dtpArrivalDate.Checked = false;
            this.Controls.Add(this.dtpArrivalDate);

            this.btnSave = new Button();
            this.btnSave.Text = "Сохранить";
            this.btnSave.Size = new Size(150, 50);
            this.btnSave.Location = new Point(120, 460);
            this.btnSave.BackColor = Color.FromArgb(70, 130, 180);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            this.Controls.Add(this.btnSave);

            this.btnCancel = new Button();
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Size = new Size(150, 50);
            this.btnCancel.Location = new Point(320, 460);
            this.btnCancel.BackColor = Color.FromArgb(180, 60, 60);
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