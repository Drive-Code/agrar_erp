using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class AddEditItemForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel dynamicPanel;
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

            this.ClientSize = new Size(620, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(32, 32, 40);
            this.ForeColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 11);
            this.Name = "AddEditItemForm";

            this.dynamicPanel = new Panel();
            this.dynamicPanel.Location = new Point(0, 0);
            this.dynamicPanel.Size = new Size(620, 400);
            this.dynamicPanel.AutoScroll = true;
            this.dynamicPanel.HorizontalScroll.Enabled = false;
            this.dynamicPanel.HorizontalScroll.Visible = false;
            this.dynamicPanel.VerticalScroll.Enabled = true;
            this.dynamicPanel.VerticalScroll.Visible = true;
            this.Controls.Add(this.dynamicPanel);

            this.btnSave = new Button();
            this.btnSave.Text = "Сохранить";
            this.btnSave.Size = new Size(200, 55);
            this.btnSave.BackColor = Color.FromArgb(70, 130, 180);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            this.Controls.Add(this.btnSave);

            this.btnCancel = new Button();
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Size = new Size(160, 55);
            this.btnCancel.BackColor = Color.FromArgb(180, 60, 60);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            this.Controls.Add(this.btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}