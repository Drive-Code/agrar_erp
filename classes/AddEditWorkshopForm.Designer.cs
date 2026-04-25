using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class AddEditWorkshopForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblName;
        private Label lblDescription;
        private TextBox txtName;
        private TextBox txtDescription;
        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.ClientSize = new Size(500, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(32, 32, 40);
            this.ForeColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 11);
            this.Name = "AddEditWorkshopForm";

            this.lblName = new Label();
            this.lblName.Text = "Название цеха:";
            this.lblName.Location = new Point(20, 30);
            this.lblName.AutoSize = true;
            this.Controls.Add(this.lblName);

            this.txtName = new TextBox();
            this.txtName.Location = new Point(160, 27);
            this.txtName.Size = new Size(310, 30);
            this.txtName.BackColor = Color.FromArgb(50, 50, 60);
            this.txtName.ForeColor = Color.WhiteSmoke;
            this.txtName.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.txtName);

            this.lblDescription = new Label();
            this.lblDescription.Text = "Описание:";
            this.lblDescription.Location = new Point(20, 80);
            this.lblDescription.AutoSize = true;
            this.Controls.Add(this.lblDescription);

            this.txtDescription = new TextBox();
            this.txtDescription.Location = new Point(160, 77);
            this.txtDescription.Size = new Size(310, 100);
            this.txtDescription.Multiline = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;
            this.txtDescription.BackColor = Color.FromArgb(50, 50, 60);
            this.txtDescription.ForeColor = Color.WhiteSmoke;
            this.txtDescription.BorderStyle = BorderStyle.FixedSingle;

            this.btnSave = new Button();
            this.btnSave.Text = "Сохранить";
            this.btnSave.Size = new Size(120, 40);
            this.btnSave.Location = new Point(140, 210);
            this.btnSave.BackColor = Color.FromArgb(70, 130, 180);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            this.Controls.Add(this.btnSave);

            this.btnCancel = new Button();
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Size = new Size(120, 40);
            this.btnCancel.Location = new Point(280, 210);
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